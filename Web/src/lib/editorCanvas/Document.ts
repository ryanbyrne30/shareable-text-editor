import { EventManager } from './EventManager';

export class Document {
	private text: string = '';
	private rows: number = 0;
	private cols: number = 0;

	public insert = (row: number, col: number, text: string) => {
		const lines = this.text.split('\n');
		const pre = lines.slice(0, row);
		const pos = lines.slice(row + 1);
		const mid = lines[row].slice(0, col) + text + lines[row].slice(col);
		this.text = [...pre, mid, ...pos].join('\n');
		this.resize();
	};

	public delete = (fromRow: number, fromCol: number, toRow: number, toCol: number) => {
		let curRow = 0;
		let curCol = 0;
		let text = '';
		console.debug({ fromRow, fromCol, toRow, toCol });
		for (let i = 0; i < this.text.length; i++) {
			const char = this.text[i];
			if (char === '\n') {
				curRow += 1;
				curCol = 0;
			} else if (i > 0) {
				curCol += 1;
			}

			console.debug({ char, curRow, curCol, i });

			if (curRow < fromRow) text += char;
			if (curRow > toRow) text += char;
			if (curRow === fromRow && curCol < fromCol) text += char;
			if (curRow === toRow && curCol > toCol) text += char;
		}
		this.text = text;
		this.resize();
	};

	public getText = () => {
		return this.text;
	};

	private resize = () => {
		const lines = this.text.split('\n');
		let max = 0;
		for (let line of lines) {
			if (line.length > max) max = line.length;
		}

		if (lines.length !== this.rows || max !== this.cols)
			EventManager.emitCanvasResizeEvent({ w: max, h: lines.length });
		this.cols = max;
		this.rows = lines.length;
	};

	public getRows = () => {
		return this.rows;
	};

	public getCols = () => {
		return this.cols;
	};

	public projectPosOnText = (row: number, col: number): { row: number; col: number } => {
		const lines = this.text.split('\n');
		if (row >= lines.length) return { row: this.rows, col: lines[this.rows - 1].length };
		if (row < 0) return { row: 0, col: 0 };
		const line = lines[row];
		if (col >= line.length) return { row, col: line.length };
		if (col < 0) return { row, col: 0 };
		return { row, col };
	};
}
