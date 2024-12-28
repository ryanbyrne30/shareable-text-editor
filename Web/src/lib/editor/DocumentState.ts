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
		return this.lastSyncedVersion + this.pendingChanges.length + 1;
	};

	#sendNextChange = (): void => {
		if (this.sentChanges !== null) return;
		const pending = this.pendingChanges.pop();
		if (pending === undefined) return;
		this.sentChanges = pending;
		this.socket.sendChange(pending);
	};

	#addPendingChange = (action: TextAction): void => {
		console.debug('Adding change:', action);
		action.revision = this.#getNextRevisionNumber();
		this.#applyLocalAction(action);
		this.pendingChanges.unshift(action);
		this.#sendNextChange();
	};

	#acknowledgeSentChange = (version: number) => {
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

		// TODO: apply OT here if version mismatch

		const action: TextAction = {
			pos: message.pos ?? 0,
			revision: message.revision ?? 0,
			delete: message.delete ?? undefined,
			insert: message.insert ?? undefined
		};
		const { sendAction, pendingActions } = OperationalTransform.transform(
			action,
			this.sentChanges,
			this.pendingChanges
		);
		this.#applyRemoteAction(message);

		for (let cb of this.onTextChange) {
			cb(this.document);
		}
	};

	#applyLocalAction = (action: TextAction) => {
		const pos = action.pos;
		const insert = action.insert;
		const del = action.delete;

		const hasInsert = typeof insert === 'string';
		const hasDelete = typeof del === 'number';

		if (hasInsert && hasDelete) this.#replace(pos, del, insert);
		else if (hasInsert) this.#insert(pos, insert);
		else if (hasDelete) this.#delete(pos, del);
	};

	#applyRemoteAction = (action: ReceivedMessage) => {
		const pos = action.pos;
		const insert = action.insert;
		const del = action.delete;
		const revision = action.revision;

		const hasInsert = typeof insert === 'string';
		const hasDelete = typeof del === 'number';
		const hasPos = typeof pos === 'number';
		const hasRevision = typeof revision === 'number';

		if (!hasPos) {
			console.error('Action does not have position:', action);
			throw new Error('Action does not have position');
		}
		if (!hasRevision) {
			console.error('Action does not have revision:', action);
			throw new Error('Action does not have revision');
		}

		this.lastSyncedVersion = revision;

		if (this.sentChanges !== null) {
			throw new Error('Not implemented. Need conflict resolution here');
		}

		if (hasInsert && hasDelete) this.#replace(pos, del, insert);
		else if (hasInsert) this.#insert(pos, insert);
		else if (hasDelete) this.#delete(pos, del);
	};

	#insert = (pos: number, text: string) => {
		console.debug(`Inserting '${text}' at pos ${pos}`);
		const before = this.document.slice(0, pos);
		const after = this.document.slice(pos);
		this.document = before + text + after;
	};

	#delete = (pos: number, length: number) => {
		console.debug(`Deleting '${length}' at pos ${pos}`);
		this.document = this.document.slice(0, pos) + this.document.slice(pos + length);
	};

	#replace = (pos: number, length: number, text: string) => {
		console.debug(`Replacing '${length}' at pos ${pos} with ${text}`);
		this.document = this.document.slice(0, pos) + text + this.document.slice(pos + length);
	};
}
