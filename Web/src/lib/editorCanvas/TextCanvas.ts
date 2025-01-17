import { EventManager, type CanvasResizeEvent } from './EventManager';

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
		EventManager.addCanvasResizeEventListener(this.onCanvasResizeEvent);
	}

	private onCanvasResizeEvent = (event: CanvasResizeEvent) => {
		this.canvasEl.width = event.w * this.colWidth + 10;
		this.canvasEl.height = event.h * (this.rowHeight + this.rowPadding);
	};

	private clearCanvas = () => {
		this.ctx.clearRect(0, 0, this.canvasEl.width, this.canvasEl.height);
	};

	public renderText = (text: string) => {
		const lines = text.split('\n');
		this.clearCanvas();
		this.ctx.font = `${this.rowHeight}px monospace`;

		for (let i = 0; i < lines.length; i++) {
			const yoff = i * (this.rowHeight + this.rowPadding) + this.rowHeight;
			const line = lines[i];
			for (let j = 0; j < line.length; j++) {
				const xoff = j * this.colWidth;
				this.ctx.fillText(line[j], xoff, yoff);
			}
		}
	};
}
