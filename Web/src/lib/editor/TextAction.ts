export type TextAction = {
	revision: number;
	pos: number;
	insert?: string;
	delete?: number;
};

export const NON_TEXT_KEYS = [
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
