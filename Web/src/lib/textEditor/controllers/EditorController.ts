import type { ITextContainer } from '../containers/textContainer';
import type { ICursorCanvas } from '../hoc/cursorCanvas';
import type { ITextCanvas } from '../hoc/textCanvas';

export interface EditorControllerConfig {
	textCanvas: ITextCanvas;
	cursorCanvas: ICursorCanvas;
	textContainer: ITextContainer;
}

export class EditorController {
	private textCanvas: ITextCanvas;
	private cursorCanvas: ICursorCanvas;
	private textContainer: ITextContainer;

	constructor(config: EditorControllerConfig) {
		this.textCanvas = config.textCanvas;
		this.cursorCanvas = config.cursorCanvas;
		this.textContainer = config.textContainer;
		this.configureEventListeners();
		this.cursorCanvas.startBlinking();
	}

	configureEventListeners = () => {
		this.textContainer.setOnInput(this.textCanvas.render);
		this.textContainer.setOnSelectionChange(({ selectionStart }) =>
			this.cursorCanvas.move(selectionStart)
		);
	};
}
