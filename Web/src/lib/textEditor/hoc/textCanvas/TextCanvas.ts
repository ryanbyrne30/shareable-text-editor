import type { ICanvas } from '$lib/textEditor/components/canvas';
import type { IGrid } from '$lib/textEditor/components/grid';
import type { Cell } from '$lib/textEditor/types/Cell';
import type { ITextCanvas } from '.';

export interface TextCanvasConfig {
	grid: IGrid;
	canvas: ICanvas;
}

export class TextCanvas implements ITextCanvas {
	private grid: IGrid;
	private canvas: ICanvas;

	constructor(config: TextCanvasConfig) {
		this.grid = config.grid;
		this.canvas = config.canvas;
	}

	renderChar = (char: string, cell: Cell) => {
		const coord = this.grid.cellToCanvasCoord(cell);
		const { rowHeight, rowPadding } = this.grid.dimensions();
		this.canvas.drawChar(char, coord.x, coord.y + rowHeight + rowPadding, { size: rowHeight });
	};

	render = (text: string): void => {
		this.canvas.clear();
		let row = 0;
		let col = 0;

		for (let ch of text) {
			if (ch === '\n') {
				row += 1;
				col = 0;
			} else {
				this.renderChar(ch, { row, col });
				col += 1;
			}
		}
	};
}
