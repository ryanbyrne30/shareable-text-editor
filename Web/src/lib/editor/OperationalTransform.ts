import { newTextAction, type TextAction } from './TextAction';

export class OperationalTransform {
	static getRevision = (newAction: TextAction, oldAction: TextAction): number => {
		return Math.max(oldAction.revision + 1, newAction.revision + 1);
	};

	static transform = (
		receivedAction: TextAction,
		sendAction: TextAction | null,
		pendingActions: TextAction[]
	): { sendAction: TextAction | null; pendingActions: TextAction[]; applyAction: TextAction } => {
		const newSendAction =
			sendAction === null ? null : this.transformAction(sendAction, receivedAction);

		let applyAction = { ...receivedAction };
		if (sendAction !== null) applyAction = this.reverseUpdateOverUpdate(sendAction, receivedAction);
		const transformedPendingActions: TextAction[] = [];
		for (let a of pendingActions) {
			transformedPendingActions.push(this.transformAction(a, receivedAction));
			applyAction = this.reverseUpdateOverUpdate(a, applyAction);
		}
		return {
			sendAction: newSendAction,
			pendingActions: transformedPendingActions,
			applyAction
		};
	};

	static transformAction = (newAction: TextAction, oldAction: TextAction): TextAction => {
		const newUpdate: TextAction = {
			pos: newAction.pos,
			revision: newAction.revision,
			delete: newAction.delete ?? 0,
			insert: newAction.insert ?? ''
		};
		const oldUpdate: TextAction = {
			pos: oldAction.pos,
			revision: oldAction.revision,
			delete: oldAction.delete ?? 0,
			insert: oldAction.insert ?? ''
		};
		return this.updateOverUpdate(newUpdate, oldUpdate);
	};

	static updateOverUpdate = (newAction: TextAction, oldAction: TextAction): TextAction => {
		const revision = this.getRevision(newAction, oldAction);
		const newDelete = newAction.delete ?? 0;
		const oldDelete = oldAction.delete ?? 0;
		const oldInsertLength = oldAction.insert?.length ?? 0;
		const oldNetLength = oldInsertLength - oldDelete;

		if (newAction.pos < oldAction.pos) {
			const positionDelta = oldAction.pos - newAction.pos;
			if (positionDelta > newDelete)
				return newTextAction(revision, newAction.pos, newAction.insert, newAction.delete);

			const deleteRemaining = Math.max(0, newDelete - positionDelta - oldDelete);
			return newTextAction(
				revision,
				newAction.pos,
				newAction.insert + (oldAction.insert ?? ''),
				positionDelta + oldInsertLength + deleteRemaining
			);
		}

		const positionDif = newAction.pos - oldAction.pos;
		if (positionDif > oldDelete)
			return newTextAction(revision, newAction.pos + oldNetLength, newAction.insert, newDelete);

		const deleteSurplus = Math.max(0, newDelete - oldDelete + positionDif);
		return newTextAction(
			revision,
			oldAction.pos + oldInsertLength,
			newAction.insert,
			deleteSurplus
		);
	};

	static reverseUpdateOverUpdate = (
		localAction: TextAction,
		remoteAction: TextAction
	): TextAction => {
		const revision = 0; // don't care about revision as remoteAction will only be applied to UI
		const localDelete = localAction.delete ?? 0;
		const localInsert = localAction.insert ?? '';
		const localInsertLength = localAction.insert?.length ?? 0;
		const localNetLength = localInsertLength - localDelete;
		const remoteDelete = remoteAction.delete ?? 0;
		const remoteInsert = remoteAction.insert ?? '';

		if (remoteAction.pos <= localAction.pos) {
			const positionDelta = localAction.pos - remoteAction.pos;
			if (positionDelta > remoteDelete)
				return newTextAction(revision, remoteAction.pos, remoteAction.insert, remoteAction.delete);

			const deleteRemainder = Math.max(0, remoteDelete - positionDelta - localDelete);
			const deleteTotal = positionDelta + localInsertLength + deleteRemainder;
			const replaceWith = remoteInsert + localInsert;
			return newTextAction(revision, remoteAction.pos, replaceWith, deleteTotal);
		}

		const positionDif = remoteAction.pos - localAction.pos;
		if (positionDif > localDelete)
			return newTextAction(
				revision,
				remoteAction.pos + localNetLength,
				remoteAction.insert,
				remoteDelete
			);

		const localEndPos = localAction.pos + localInsertLength;
		const deleteTotal = Math.max(0, remoteDelete - localDelete + positionDif);
		return newTextAction(revision, localEndPos, remoteInsert, deleteTotal);
	};

	static applyAction = (text: string, action: TextAction): string => {
		const insert = action.insert ?? '';
		const del = action.delete ?? 0;

		const before = text.slice(0, action.pos);
		const middle = insert;
		const after = text.slice(action.pos + del);
		return before + middle + after;
	};
}
