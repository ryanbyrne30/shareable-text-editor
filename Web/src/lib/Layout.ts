export interface LayoutConfig {
	canvas: HTMLCanvasElement;
	canvasPaddingX?: number;
	canvasPaddingY?: number;
	rowSize?: number;
	colSize?: number;
	rowPadding?: number;
}

export class Layout {
	private canvas: HTMLCanvasElement;
	private canvasPaddingX: number;
	private canvasPaddingY: number;
	private rowSize: number;
	private colSize: number;
	private rowPadding: number;

	constructor(config: LayoutConfig) {
		this.canvas = config.canvas;
		this.canvasPaddingX = config.canvasPaddingX ?? 0;
		this.canvasPaddingY = config.canvasPaddingY ?? 0;
		this.rowSize = config.rowSize ?? 16;
		this.colSize = config.colSize ?? 12;
		this.rowPadding = config.rowPadding ?? 0;
	}

	canvasDims = () => {
		return this.canvas.getBoundingClientRect();
	};

	cellToCanvasCoord = (row: number, col: number) => {
		const yPad = this.rowPadding * row + this.canvasPaddingY;
		const xPad = this.canvasPaddingX;
		const y = row * this.rowSize + yPad;
		const x = col * this.colSize + xPad;
		return { x, y };
	};

	screenPosToCell = (x: number, y: number) => {
		const canvasDim = this.canvasDims();
		const canvasX = canvasDim.x;
		const canvasY = canvasDim.y;
		const relativeX = x - this.canvasPaddingX - canvasX;
		const relativeY = y - this.canvasPaddingY - canvasY;
		const rowHeight = this.rowSize + this.rowPadding;
		const row = Math.floor(relativeY / rowHeight);
		const col = Math.round(relativeX / this.colSize);
		return { row, col };
	};

	screenPosToCanvasCoord = (x: number, y: number) => {
		const { row, col } = this.screenPosToCell(x, y);
		return this.cellToCanvasCoord(row, col);
	};

	adjustCanvasCoordForText = (x: number, y: number) => {
		return {
			x,
			y: y + this.rowSize
		};
	};
}
