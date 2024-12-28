import { OperationalTransform } from './OperationalTransform';
import type { ReceivedMessage, SyncSocket } from './SyncSocket';
import { NON_TEXT_KEYS, type TextAction } from './TextAction';

export type TextChangeCallback = (text: string) => void;

export class DocumentState {
	private lastSyncedVersion: number = 0;
	private pendingChanges: TextAction[] = [];
	private sentChanges: TextAction | null = null;
	private document: string = '';
	private onTextChange: TextChangeCallback[] = [];

	constructor(private socket: SyncSocket) {}

	#getNextRevisionNumber = (): number => {
		const add = this.sentChanges === null ? 0 : 1;
		return this.lastSyncedVersion + this.pendingChanges.length + add + 1;
	};

	#sendNextChange = (): void => {
		if (this.sentChanges !== null) return;
		const pending = this.pendingChanges.pop();
		if (pending === undefined) return;
		this.sentChanges = { ...pending };
		this.socket.sendChange(this.sentChanges);
	};

	#addPendingChange = (action: TextAction): void => {
		console.debug('Adding change:', action);
		action.revision = this.#getNextRevisionNumber();
		this.#applyLocalAction(action);
		this.pendingChanges.unshift(action);
		this.#sendNextChange();
	};

	#acknowledgeSentChange = (version: number) => {
		console.debug('Sent change acknowledged. Synced version:', version);
		this.lastSyncedVersion = version;
		this.sentChanges = null;
		this.#sendNextChange();
	};

	addTextChangeCallback = (cb: TextChangeCallback) => {
		this.onTextChange.push(cb);
	};

	addChangeFromSelection = (start: number, end: number, keyPressed: string): void => {
		const revision = this.#getNextRevisionNumber();
		const isNonTextKey =
			keyPressed !== 'Backspace' && keyPressed !== 'Delete' && NON_TEXT_KEYS.includes(keyPressed);

		if (isNonTextKey) return;

		let action: TextAction | undefined = undefined;

		switch (keyPressed) {
			case 'Backspace':
				action = {
					revision,
					pos: start,
					delete: end - start
				};
				break;
			case 'Delete':
				action = {
					revision,
					pos: start,
					delete: end - start
				};
				break;
			default:
				action = {
					revision,
					pos: start,
					delete: end - start,
					insert: keyPressed
				};
		}
		this.#addPendingChange(action);
	};

	addChangeFromNonSelection = (pos: number, keyPressed: string): void => {
		const revision = this.#getNextRevisionNumber();
		const isBackspaceAtBeginning = keyPressed === 'Backspace' && pos === 0;
		const isDeleteAtEnd = keyPressed === 'Delete' && this.document.length === pos;
		const isNonTextKey =
			keyPressed !== 'Backspace' && keyPressed !== 'Delete' && NON_TEXT_KEYS.includes(keyPressed);

		if (isBackspaceAtBeginning || isDeleteAtEnd || isNonTextKey) return;

		let action: TextAction | undefined = undefined;

		switch (keyPressed) {
			case 'Backspace':
				action = {
					revision,
					pos: pos - 1,
					delete: 1
				};
				break;
			case 'Delete':
				action = {
					revision,
					pos,
					delete: 1
				};
				break;
			default:
				action = {
					revision,
					pos,
					insert: keyPressed
				};
		}

		this.#addPendingChange(action);
	};

	resolveMessage = (message: ReceivedMessage): void => {
		if (message.ack?.success) {
			this.#acknowledgeSentChange(message.ack.version);
			return;
		}
		console.debug('Received message:', message);
		console.log('Previous revision:', this.lastSyncedVersion, 'New revision:', message.revision);
		this.lastSyncedVersion = message.revision ?? this.lastSyncedVersion;

		const action: TextAction = {
			pos: message.pos ?? 0,
			revision: message.revision ?? 0,
			delete: message.delete ?? undefined,
			insert: message.insert ?? undefined
		};
		const { sendAction, pendingActions, applyAction } = OperationalTransform.transform(
			action,
			this.sentChanges,
			this.pendingChanges
		);
		console.log('State:', {
			version: this.lastSyncedVersion,
			received: message,
			apply: applyAction,
			sentOriginal: this.sentChanges,
			sent: sendAction,
			pendingOriginal: this.pendingChanges,
			pending: pendingActions
		});
		this.#applyRemoteAction(applyAction);

		this.sentChanges = sendAction;
		this.pendingChanges = pendingActions;

		for (let cb of this.onTextChange) {
			cb(this.document);
		}
	};

	#applyLocalAction = (action: TextAction) => {
		this.document = OperationalTransform.applyAction(this.document, {
			revision: action.revision ?? 0,
			pos: action.pos ?? 0,
			delete: action.delete ?? 0,
			insert: action.insert ?? ''
		});
	};

	#applyRemoteAction = (action: ReceivedMessage) => {
		if (action.pos === undefined) {
			console.error('Action does not have position:', action);
			throw new Error('Action does not have position');
		}
		if (action.revision === undefined) {
			console.error('Action does not have revision:', action);
			throw new Error('Action does not have revision');
		}

		this.document = OperationalTransform.applyAction(this.document, {
			revision: action.revision ?? 0,
			pos: action.pos ?? 0,
			delete: action.delete ?? 0,
			insert: action.insert ?? ''
		});
	};
}
