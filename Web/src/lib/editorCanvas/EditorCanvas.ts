import { CursorCanvas } from './CursorCanvas';
import { Document } from './Document';
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
		const document = new Document();
		const textCanvas = new TextCanvas(
			this.textHeight,
			this.textWidth,
			this.textRowPadding,
			textCanvasEl
		);
		const cursorCanvas = new CursorCanvas(
			this.textHeight,
			this.textWidth,
			this.textRowPadding,
			cursorCanvasEl
		);
		const inputController = new InputController(textCanvas, cursorCanvas, document);
	}
}
