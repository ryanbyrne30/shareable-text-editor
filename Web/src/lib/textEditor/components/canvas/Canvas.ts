import type { ICanvas } from '.';

export class Canvas implements ICanvas {
	private ctx: CanvasRenderingContext2D;

	constructor(private canvas: HTMLCanvasElement) {
		const ctx = canvas.getContext('2d');
		if (!ctx) throw new Error('2D context not available for canvas');
		this.ctx = ctx;
	}

	drawRect(x: number, y: number, width: number, height: number, opts?: { style: string }): void {
		if (opts?.style) this.ctx.fillStyle = opts.style;
		this.ctx.fillRect(x, y, width, height);
	}

	clearRect(x: number, y: number, width: number, height: number): void {
		this.ctx.clearRect(x, y, width, height);
	}

	clear(): void {
		const { height, width } = this.canvas.getBoundingClientRect();
		this.ctx.clearRect(0, 0, width, height);
	}

	drawChar(char: string, x: number, y: number, opts?: { style?: string; size?: number }): void {
		if (opts?.style) this.ctx.fillStyle = opts.style;
		if (opts?.size) this.ctx.font = `${opts.size}px mono`;
		this.ctx.fillText(char, x, y);
	}
}
