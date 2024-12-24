import type { Cell } from '$lib/textEditor/types/Cell';

export type OnSelectionChangeCallback = (args: {
	selectionStart: Cell;
	selectionEnd: Cell;
	direction: 'forward' | 'backward' | 'none';
}) => void;

export type OnKeydownCallback = (e: KeyboardEvent) => void;

export type OnInputCallback = (input: string) => void;
