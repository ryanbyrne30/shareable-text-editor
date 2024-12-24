import type { Cell } from '$lib/textEditor/types/Cell';
import type { Coord } from '$lib/textEditor/types/Coord';
import type { Dimensions, IGrid } from '.';

export interface GridConfig {
	canvas: HTMLCanvasElement;
	canvasPaddingX?: number;
	canvasPaddingY?: number;
	rowHeight?: number;
	colWidth?: number;
	rowPadding?: number;
}

export class Grid implements IGrid {
	private canvas: HTMLCanvasElement;
	private canvasPaddingX: number;
	private canvasPaddingY: number;
	private rowSize: number;
	private colSize: number;
	private rowPadding: number;

	constructor(config: GridConfig) {
		this.canvas = config.canvas;
		this.canvasPaddingX = config.canvasPaddingX ?? 0;
		this.canvasPaddingY = config.canvasPaddingY ?? 0;
		this.rowSize = config.rowHeight ?? 16;
		this.colSize = config.colWidth ?? 12;
		this.rowPadding = config.rowPadding ?? 0;
	}

	dimensions = (): Dimensions => {
		const rect = this.canvas.getBoundingClientRect();
		return {
			left: rect.left,
			right: rect.right,
			top: rect.top,
			bottom: rect.bottom,
			width: rect.width,
			height: rect.height,
			colWidth: this.colSize,
			rowHeight: this.rowSize,
			paddingX: this.canvasPaddingX,
			paddingY: this.canvasPaddingY,
			rowPadding: this.rowPadding
		};
	};

	cellToCanvasCoord(cell: Cell): Coord {
		const yPad = this.rowPadding * cell.row + this.canvasPaddingY;
		const xPad = this.canvasPaddingX;
		const y = cell.row * this.rowSize + yPad;
		const x = cell.col * this.colSize + xPad;
		return { x, y };
	}

	screenCoordToCell = (coord: Coord): Cell => {
		const canvasDim = this.dimensions();
		const canvasX = canvasDim.left;
		const canvasY = canvasDim.top;
		const relativeX = coord.x - this.canvasPaddingX - canvasX;
		const relativeY = coord.y - this.canvasPaddingY - canvasY;
		const rowHeight = this.rowSize + this.rowPadding;
		const row = Math.floor(relativeY / rowHeight);
		const col = Math.round(relativeX / this.colSize);
		return { row, col };
	};
}
