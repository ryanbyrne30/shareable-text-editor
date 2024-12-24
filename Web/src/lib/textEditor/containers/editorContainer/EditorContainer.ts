import type { ITextContainer } from '../textContainer';
import type { ICursorCanvas } from '../cursorCanvas';
import type { ITextCanvas } from '../textCanvas';

export interface EditorContainerConfig {
	textCanvas: ITextCanvas;
	cursorCanvas: ICursorCanvas;
	textContainer: ITextContainer;
}

export class EditorContainer {
	private textCanvas: ITextCanvas;
	private cursorCanvas: ICursorCanvas;
	private textContainer: ITextContainer;

	constructor(config: EditorContainerConfig) {
		this.textCanvas = config.textCanvas;
		this.cursorCanvas = config.cursorCanvas;
		this.textContainer = config.textContainer;
		this.configureEventListeners();
		this.cursorCanvas.startBlinking();
	}

	configureEventListeners = () => {
		this.textContainer.setOnInput((input) => {
			this.textCanvas.render(input);
			const { end } = this.textContainer.getCurrentSelection();
			this.cursorCanvas.move(end);
		});
		this.textContainer.setOnSelectionChange(({ selection, direction }) => {
			this.cursorCanvas.select(selection, direction === 'backward' ? -1 : 1, {
				style: 'lightgray'
			});
		});
	};
}
