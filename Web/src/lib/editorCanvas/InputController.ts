import type { CursorCanvas } from './CursorCanvas';
import type { TextCanvas } from './TextCanvas';

export class InputController {
	private text: string = '';

	constructor(
		private textCanvas: TextCanvas,
		private cursorCanvas: CursorCanvas
	) {
		this.setup();
	}

	private setup = () => {
		console.debug('Setting up input controller');
		document.addEventListener('keydown', this.onKeyDown);
	};

	private onKeyDown = (e: KeyboardEvent) => {
		const key = e.key;
		if (key === 'Enter') this.text += '\n';
		else this.text += key;
		this.textCanvas.renderText(this.text);
	};
}
