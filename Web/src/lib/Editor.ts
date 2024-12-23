import { BlinkerCanvas } from './BlinkerCanvas';
import { EditorCanvas } from './EditorCanvas';
import { Layout } from './Layout';

export interface EditorConfig {
	editorCanvas: HTMLCanvasElement;
	blinkerCanvas: HTMLCanvasElement;
	textarea: HTMLTextAreaElement;
}

export class Editor {
	private rowSize = 16;
	private rowPadding = 4;
	private textarea: HTMLTextAreaElement;
	private layout: Layout;
	private editor: EditorCanvas;
	private blinker: BlinkerCanvas;
	private cursorRow = 0;
	private cursorCol = 0;

	constructor(config: EditorConfig) {
		const editorContext = config.editorCanvas.getContext('2d');
		const blinkerContext = config.blinkerCanvas.getContext('2d');
		if (!editorContext) throw new Error('No context for editor canvas');
		if (!blinkerContext) throw new Error('No context for blinker canvas');

		this.textarea = config.textarea;

		this.layout = new Layout({
			canvas: config.editorCanvas,
			canvasPaddingX: 0,
			canvasPaddingY: 0,
			rowPadding: this.rowPadding,
			rowSize: this.rowSize
		});

		this.blinker = new BlinkerCanvas({
			layout: this.layout,
			context: blinkerContext,
			height: this.rowSize + this.rowPadding,
			width: 4,
			timeoutMs: 500,
			cellCol: this.cursorCol,
			cellRow: this.cursorRow
		});

		this.editor = new EditorCanvas({
			layout: this.layout,
			context: editorContext
		});

		document.addEventListener('keydown', this.moveBlinker);
		this.textarea.addEventListener('selectionchange', this.moveBlinker);
		config.blinkerCanvas.addEventListener('mousedown', (e) => {
			this.onClick(e.clientX, e.clientY);
		});
	}

	moveBlinker = () => {
		this.blinker.stop();
		const charPos = this.textarea.selectionEnd;
		const cell = this.editor.getCellForTextCharPos(charPos);
		this.cursorCol = cell.col;
		this.cursorRow = cell.row;
		this.blinker.move(this.cursorRow, this.cursorCol);
		this.blinker.start();
	};

	onClick = (clickX: number, clickY: number) => {
		setTimeout(() => {
			this.textarea.focus();
			const charPos = this.editor.getTextCharPosForCell(this.cursorRow, this.cursorCol);
			this.textarea.setSelectionRange(charPos, charPos);
		}, 0);
		const cell = this.layout.screenPosToCell(clickX, clickY);
		const targetCell = this.editor.getNearestTextCellToCell(cell.row, cell.col);
		const targetChar = this.editor.getTextCharPosForCell(targetCell.row, targetCell.col);
		setTimeout(() => {
			this.textarea.setSelectionRange(targetChar, targetChar);
		}, 0);
	};

	writeText = (text: string) => {
		this.editor.clear();
		this.editor.writeText(text);
	};
}
