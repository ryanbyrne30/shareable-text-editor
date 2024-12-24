import type { Cell } from '$lib/textEditor/types/Cell';
import type { Coord } from '$lib/textEditor/types/Coord';

export type Dimensions = {
	left: number;
	right: number;
	top: number;
	bottom: number;
	width: number;
	height: number;
	rowHeight: number;
	colWidth: number;
	rowPadding: number;
	paddingX: number;
	paddingY: number;
};

export interface IGrid {
	screenCoordToCell(coord: Coord): Cell;
	cellToCanvasCoord(cell: Cell): Coord;
	dimensions(): Dimensions;
}
