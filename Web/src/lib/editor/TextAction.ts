export type InsertAction = {
	retainBefore: number;
	insert: string;
	retainAfter: number;
};

export type DeleteAction = {
	retainBefore: number;
	delete: number;
	retainAfter: number;
};

export type UpdateAction = {
	retainBefore: number;
	delete: number;
	insert: string;
	retainAfter: number;
};

export const nonTextKeys = [
	'Escape',
	'F1',
	'F2',
	'F3',
	'F4',
	'F5',
	'F6',
	'F7',
	'F8',
	'F9',
	'F10',
	'F11',
	'F12',
	'Insert',
	'Delete',
	'Home',
	'End',
	'PageUp',
	'PageDown',
	'ArrowUp',
	'ArrowDown',
	'ArrowLeft',
	'ArrowRight',
	'Shift',
	'Control',
	'Alt',
	'Meta', // Meta is the Command key on macOS
	'Backspace',
	'Tab',
	'CapsLock',
	'NumLock',
	'ScrollLock',
	'Pause',
	'PrintScreen',
	'ContextMenu'
];

export type TextActionType = InsertAction | DeleteAction | UpdateAction;

export class TextAction {
	constructor(private action: TextActionType) {}

	public static fromSelection = (
		start: number,
		end: number,
		textLength: number,
		keyPressed: string
	): TextAction | null => {
		const retainBefore = start;
		const deleted = end - start;
		const retainAfter = textLength - end;

		const deleteAction: DeleteAction = {
			retainBefore,
			delete: deleted,
			retainAfter
		};
		if (keyPressed === 'Backspace' || keyPressed === 'Delete') return new TextAction(deleteAction);

		if (nonTextKeys.includes(keyPressed)) return null;

		const updateAction: UpdateAction = {
			retainBefore,
			delete: deleted,
			insert: keyPressed,
			retainAfter
		};
		return new TextAction(updateAction);
	};

	public static fromNonSelection = (
		pos: number,
		textLength: number,
		keyPressed: string
	): TextAction | null => {
		if (keyPressed === 'Backspace') {
			const deleteAction: DeleteAction = {
				retainBefore: pos - 1,
				delete: 1,
				retainAfter: textLength - pos
			};
			return new TextAction(deleteAction);
		}

		if (keyPressed === 'Delete' && textLength === pos) {
			return null;
		}

		if (keyPressed === 'Delete') {
			const deleteAction: DeleteAction = {
				retainBefore: pos,
				delete: 1,
				retainAfter: textLength - pos - 1
			};
			return new TextAction(deleteAction);
		}

		if (nonTextKeys.includes(keyPressed)) return null;

		const insertAction: InsertAction = {
			retainBefore: pos,
			insert: keyPressed,
			retainAfter: textLength - pos
		};
		return new TextAction(insertAction);
	};
}
