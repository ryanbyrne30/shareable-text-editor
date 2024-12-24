import type { Cell } from '$lib/textEditor/types/Cell';

export interface ICursorCanvas {
	startBlinking(): void;
	stopBlinking(): void;
	move(cell: Cell): void;
	select(cells: Cell[], direction: number, opts?: { style?: string }): void;
}
