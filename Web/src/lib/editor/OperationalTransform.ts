import {
	newTextAction,
	textActionIsDelete,
	textActionIsInsert,
	textActionIsUpdate,
	type TextAction
} from './TextAction';

export class OperationalTransform {
	static getRevision = (newAction: TextAction, oldAction: TextAction): number => {
		return Math.max(oldAction.revision + 1, newAction.revision + 1);
	};

	static transform = (
		receivedAction: TextAction,
		sendAction: TextAction | null,
		pendingActions: TextAction[]
	): { sendAction: TextAction | null; pendingActions: TextAction[] } => {
		const newSendAction =
			sendAction === null ? null : this.transformAction(sendAction, receivedAction);

		const transformedPendingActions: TextAction[] = [];
		for (let a of pendingActions) {
			transformedPendingActions.push(this.transformAction(a, receivedAction));
		}
		return {
			sendAction: newSendAction,
			pendingActions: transformedPendingActions
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
}
