import type { CursorCanvas } from './CursorCanvas';
import { EventManager } from './EventManager';
import type { TextCanvas } from './TextCanvas';

export class InputController {
	private text: string = '';
	private cursorTimeoutMs = 500;
	private cursorTimeout = 0;

	constructor(
		private textCanvas: TextCanvas,
		private cursorCanvas: CursorCanvas
	) {
		this.setup();
	}

	private setup = () => {
		console.debug('Setting up input controller');
		document.addEventListener('keydown', this.onKeyDown);
		this.startBlinkingCursor();
	};

	private startBlinkingCursor = () => {
		clearTimeout(this.cursorTimeout);
		this.cursorCanvas.draw();
		this.cursorTimeout = setTimeout(this.clearCursor, this.cursorTimeoutMs);
	};

	private clearCursor = () => {
		clearTimeout(this.cursorTimeout);
		this.cursorCanvas.clear();
		this.cursorTimeout = setTimeout(this.startBlinkingCursor, this.cursorTimeoutMs);
	};

	private onKeyDown = (e: KeyboardEvent) => {
		this.clearCursor();
		const key = e.key;
		const lines = this.text.split('\n');
		if (key === 'Enter') {
			this.text += '\n';
			EventManager.emitCursorMoveEvent({ x: 0, y: lines.length });
		} else {
			this.text += key;
			EventManager.emitCursorMoveEvent({
				x: lines[lines.length - 1].length + 1,
				y: lines.length - 1
			});
		}
		this.textCanvas.renderText(this.text);
	};
}
