import type { SyncPublisher } from './SyncPublisher';
import { TextAction } from './TextAction';

export class TextContainer {
	private isMouseDown = false;

	constructor(
		private textarea: HTMLTextAreaElement,
		private publisher: SyncPublisher
	) {
		this.setup();
	}

	setup = () => {
		this.textarea.addEventListener('mousedown', this.#onMouseDown);
		this.textarea.addEventListener('mouseup', this.#onMouseUp);
		this.textarea.addEventListener('mousemove', this.#onMouseMove);
		this.textarea.addEventListener('keydown', this.#onKeyDown);
	};

	isFocus = () => {
		return document.activeElement === this.textarea;
	};

	getSelection = (): { start: number; end: number; direction: number } => {
		const start = this.textarea.selectionStart;
		const end = this.textarea.selectionEnd;
		const direction = this.textarea.selectionDirection;
		let directionNumber = 0;
		if (direction === 'backward') directionNumber = -1;
		if (direction === 'forward') directionNumber = 1;

		return {
			start,
			end,
			direction: directionNumber
		};
	};

	getText = (): string => {
		return this.textarea.value;
	};

	#updateCursor = () => {
		// console.log('Update cursor UI:', this.getSelection());
	};

	#onMouseDown = () => {
		this.isMouseDown = true;
		this.#updateCursor();
	};

	#onMouseUp = () => {
		this.isMouseDown = false;
		this.#updateCursor();
	};

	#onMouseMove = () => {
		if (this.isMouseDown) this.#updateCursor();
	};

	#onKeyDown = (e: KeyboardEvent) => {
		this.#updateCursor();
		this.#createAction(e);
	};

	#createAction = (e: KeyboardEvent) => {
		const selection = this.getSelection();

		let action: TextAction | null = null;
		if (selection.start < selection.end)
			action = TextAction.fromSelection(
				selection.start,
				selection.end,
				this.getText().length,
				e.key
			);
		else {
			action = TextAction.fromNonSelection(selection.start, this.getText().length, e.key);
		}

		if (action === null) return;
		this.publisher.publish(action);
	};
}
