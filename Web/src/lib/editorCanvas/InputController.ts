import type { CursorCanvas } from './CursorCanvas';
import type { Document } from './Document';
import { EventManager } from './EventManager';
import { InputKeys } from './InputKeys';
import type { TextCanvas } from './TextCanvas';

export class InputController {
	private cursorTimeoutMs = 500;
	private cursorTimeout = 0;

	constructor(
		private textCanvas: TextCanvas,
		private cursorCanvas: CursorCanvas,
		private document: Document
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
		const { row, col } = this.cursorCanvas.getPos();
		if (key === InputKeys.Enter) {
			this.document.insert(row, col, '\n');
			this.cursorCanvas.move(row + 1, 0);
		} else if (key === InputKeys.ArrowLeft) {
			const cursorPos = this.cursorCanvas.getPos();
			const { row, col } = this.document.projectPosOnText(cursorPos.row, cursorPos.col - 1);
			this.cursorCanvas.move(row, col);
			this.startBlinkingCursor();
		} else if (key === InputKeys.ArrowRight) {
			const cursorPos = this.cursorCanvas.getPos();
			const { row, col } = this.document.projectPosOnText(cursorPos.row, cursorPos.col + 1);
			this.cursorCanvas.move(row, col);
			this.startBlinkingCursor();
		} else if (key === InputKeys.ArrowUp) {
			const cursorPos = this.cursorCanvas.getPos();
			const { row, col } = this.document.projectPosOnText(cursorPos.row - 1, cursorPos.col);
			this.cursorCanvas.move(row, col);
			this.startBlinkingCursor();
		} else if (key === InputKeys.ArrowDown) {
			const cursorPos = this.cursorCanvas.getPos();
			const { row, col } = this.document.projectPosOnText(cursorPos.row + 1, cursorPos.col);
			this.cursorCanvas.move(row, col);
			this.startBlinkingCursor();
		} else if (key === InputKeys.Backspace) {
			const cursorPos = this.cursorCanvas.getPos();
			let fromRow = cursorPos.row;
			let fromCol = cursorPos.col - 1;
			if (cursorPos.row === 0 && cursorPos.col === 0) return;
			if (cursorPos.col === 0) {
				const prevPos = this.document.projectPosOnText(cursorPos.row - 1, Number.MAX_SAFE_INTEGER);
				fromRow = prevPos.row;
				fromCol = prevPos.col;
			}
			console.debug({ fromRow, fromCol });
			this.document.delete(fromRow, fromCol, fromRow, fromCol);
			const { row, col } = this.document.projectPosOnText(fromRow, fromCol);
			this.cursorCanvas.move(row, col);
			this.startBlinkingCursor();
		} else {
			this.document.insert(row, col, key);
			this.cursorCanvas.move(row, col + 1);
		}
		this.textCanvas.renderText(this.document.getText());
	};
}
