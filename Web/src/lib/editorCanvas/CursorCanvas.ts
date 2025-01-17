import { EventManager, type CanvasResizeEvent, type CursorMoveEvent } from './EventManager';

export class CursorCanvas {
	private ctx: CanvasRenderingContext2D;
	private row: number = 0;
	private col: number = 0;
	private cursorWidth: number = 4;

	constructor(
		private rowHeight: number,
		private colWidth: number,
		private rowPadding: number,
		private canvasEl: HTMLCanvasElement
	) {
		const ctx = canvasEl.getContext('2d');
		if (ctx === null) throw new Error('Could not create context for cursor canvas element');
		this.ctx = ctx;
		EventManager.addCursorMoveEventListener(this.onCursorMoveEvent);
		EventManager.addCanvasResizeEventListener(this.onCanvasResizeEvent);
	}

	public clear = () => {
		this.ctx.clearRect(0, 0, this.canvasEl.width, this.canvasEl.height);
	};

	private getCoord = (): { x: number; y: number } => {
		const x = this.col * this.colWidth;
		const y = this.row * (this.rowHeight + this.rowPadding) + this.rowPadding;
		return { x, y };
	};

	private onCanvasResizeEvent = (event: CanvasResizeEvent) => {
		this.canvasEl.width = event.w * this.colWidth + 10;
		this.canvasEl.height = event.h * (this.rowHeight + this.rowPadding);
	};

	public draw = () => {
		this.ctx.fillStyle = 'gray';
		const { x, y } = this.getCoord();
		this.ctx.fillRect(x, y, this.cursorWidth, this.rowHeight);
	};

	public move = (row: number, col: number) => {
		this.row = row;
		this.col = col;
	};

	private onCursorMoveEvent = (event: CursorMoveEvent) => {
		this.move(event.y, event.x);
	};

	public getPos = (): { row: number; col: number } => {
		return { row: this.row, col: this.col };
	};
}
