export class CursorCanvas {
	private ctx: CanvasRenderingContext2D;

	constructor(private canvasEl: HTMLCanvasElement) {
		const ctx = canvasEl.getContext('2d');
		if (ctx === null) throw new Error('Could not create context for cursor canvas element');
		this.ctx = ctx;
	}
}
