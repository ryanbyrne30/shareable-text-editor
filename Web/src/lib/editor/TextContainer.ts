export type OnChangeFromSelectionCallback = (start: number, end: number, key: string) => void;
export type OnChangeFromNonSelectionCallback = (pos: number, key: string) => void;

export class TextContainer {
	private isMouseDown = false;
	private onChangeFromSelection: OnChangeFromSelectionCallback[] = [];
	private onChangeFromNonSelection: OnChangeFromNonSelectionCallback[] = [];

	constructor(private textarea: HTMLTextAreaElement) {
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

	setText = (text: string): void => {
		this.textarea.value = text;
	};

	addChangeFromSelection = (cb: OnChangeFromSelectionCallback) => {
		this.onChangeFromSelection.push(cb);
	};

	addChangeFromNonSelection = (cb: OnChangeFromNonSelectionCallback) => {
		this.onChangeFromNonSelection.push(cb);
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
		this.#handleAction(e);
	};

	#handleAction = (e: KeyboardEvent) => {
		const selection = this.getSelection();
		if (selection.start < selection.end) {
			for (let cb of this.onChangeFromSelection) {
				cb(selection.start, selection.end, e.key);
			}
		} else {
			for (let cb of this.onChangeFromNonSelection) {
				cb(selection.start, e.key);
			}
		}
	};
}
