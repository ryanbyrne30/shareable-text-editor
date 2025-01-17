import { CursorCanvas } from './CursorCanvas';
import { InputController } from './InputController';
import { TextCanvas } from './TextCanvas';

export class EditorCanvas {
	private textHeight: number = 16;
	private textWidth: number = 10;
	private textRowPadding: number = 4;

	constructor(
		private height: number,
		private width: number,
		private textCanvasEl: HTMLCanvasElement,
		private cursorCanvasEl: HTMLCanvasElement
	) {
		const textCanvas = new TextCanvas(
			this.textHeight,
			this.textWidth,
			this.textRowPadding,
			textCanvasEl
		);
		const cursorCanvas = new CursorCanvas(cursorCanvasEl);
		const inputController = new InputController(textCanvas, cursorCanvas);
	}
}
