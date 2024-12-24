import type { ICanvas } from '$lib/textEditor/components/canvas';
import type { IGrid } from '$lib/textEditor/components/grid';
import type { Cell } from '$lib/textEditor/types/Cell';
import type { ICursorCanvas } from '.';

export interface CursorCanvasConfig {
	grid: IGrid;
	canvas: ICanvas;
}

export class CursorCanvas implements ICursorCanvas {
	private timeoutMs = 500;
	private fill = 'gray';
	private grid: IGrid;
	private canvas: ICanvas;
	private curCell: Cell = { col: 0, row: 0 };
	private blinkingTimeout = 0;

	constructor(config: CursorCanvasConfig) {
		this.grid = config.grid;
		this.canvas = config.canvas;
	}

	width = () => {
		return 4;
	};

	height = () => {
		const dims = this.grid.dimensions();
		return dims.rowHeight + dims.rowPadding;
	};

	draw = () => {
		clearTimeout(this.blinkingTimeout);
		const coord = this.grid.cellToCanvasCoord(this.curCell);
		this.canvas.drawRect(coord.x, coord.y, this.width(), this.height(), { style: this.fill });
		this.blinkingTimeout = setTimeout(this.clear, this.timeoutMs);
	};

	clear = () => {
		clearTimeout(this.blinkingTimeout);
		const coord = this.grid.cellToCanvasCoord(this.curCell);
		this.canvas.clearRect(coord.x, coord.y, this.width(), this.height(), { style: this.fill });
		this.blinkingTimeout = setTimeout(this.draw, this.timeoutMs);
	};

	move = (cell: Cell): void => {
		this.stopBlinking();
		this.curCell = cell;
		this.startBlinking();
	};

	startBlinking = (): void => {
		this.draw();
	};

	stopBlinking = (): void => {
		clearTimeout(this.blinkingTimeout);
		this.canvas.clear();
	};
}
