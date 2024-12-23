import type { Layout } from './Layout';

export interface EditorCanvasConfig {
	layout: Layout;
	context: CanvasRenderingContext2D;
	charHeight?: number;
	charWidth?: number;
	charPaddingY?: number;
}

export class EditorCanvas {
	private layout: Layout;
	private ctx: CanvasRenderingContext2D;
	private charHeight: number;
	private text: string[] = [];

	constructor(config: EditorCanvasConfig) {
		this.layout = config.layout;
		this.ctx = config.context;
		this.charHeight = config.charHeight ?? 16;
	}

	clear = () => {
		const { width, height } = this.layout.canvasDims();
		this.ctx.clearRect(0, 0, width, height);
		this.ctx.font = `${this.charHeight}px mono`;
	};

	writeChar = (char: string, row: number, col: number) => {
		const { x, y } = this.layout.cellToCanvasCoord(row, col);
		this.ctx.fillText(char, x, y + this.charHeight);
	};

	writeText = (text: string): { row: number; col: number } => {
		let r = 0;
		let c = 0;
		for (let i = 0; i < text.length; i++) {
			const ch = text.charAt(i);
			if (ch === '\n') {
				r += 1;
				c = 0;
			} else {
				this.writeChar(ch, r, c);
				c += 1;
			}
		}
		this.text = text.split('\n');
		for (let i = 0; i < this.text.length - 1; i++) {
			this.text[i] += '\n';
		}
		return { row: r, col: c };
	};

	getNearestTextCellToCell = (row: number, col: number): { row: number; col: number } => {
		const lastRow = this.text.length - 1;
		const targetRow = row > lastRow ? lastRow : row;
		const colsInTargetRow = this.text[targetRow].length;
		let targetCol = col;
		if (col >= colsInTargetRow && targetRow < this.text.length - 1) targetCol = colsInTargetRow - 1;
		else if (col >= colsInTargetRow) targetCol = colsInTargetRow;
		return { row: targetRow, col: targetCol };
	};

	getCellForTextCharPos = (pos: number): { row: number; col: number } => {
		let count = pos;
		for (let i = 0; i < this.text.length; i++) {
			const cols = this.text[i].length;
			if (cols > count) {
				return { row: i, col: count };
			}
			count -= cols;
		}
		return { row: this.text.length - 1, col: this.text[this.text.length - 1].length };
	};

	getTextCharPosForCell = (row: number, col: number): number => {
		let count = 0;
		const targetRow = Math.min(row, this.text.length - 1);
		for (let i = 0; i < targetRow; i++) {
			count += this.text[i].length;
		}
		return count + col;
	};
}
