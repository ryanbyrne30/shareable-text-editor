export class TextCanvas {
	private ctx: CanvasRenderingContext2D;

	constructor(
		private rowHeight: number,
		private colWidth: number,
		private rowPadding: number,
		private canvasEl: HTMLCanvasElement
	) {
		const ctx = canvasEl.getContext('2d');
		if (ctx === null) throw new Error('Could not create context for text canvas element');
		this.ctx = ctx;
	}

	private scaleCanvasToText = (lines: string[]) => {
		let max = 0;
		for (let line of lines) {
			if (line.length > max) max = line.length;
		}
		this.canvasEl.width = max * this.colWidth;
		this.canvasEl.height = lines.length * (this.rowHeight + this.rowPadding);
	};

	private clearCanvas = () => {
		this.ctx.clearRect(0, 0, this.canvasEl.width, this.canvasEl.height);
	};

	public renderText = (text: string) => {
		const lines = text.split('\n');
		this.scaleCanvasToText(lines);
		this.clearCanvas();
		this.ctx.font = `${this.rowHeight}px monospace`;

		for (let i = 0; i < lines.length; i++) {
			const yoff = i * (this.rowHeight + this.rowPadding) + this.rowHeight;
			this.ctx.fillText(lines[i], 0, yoff);
		}
	};
}
