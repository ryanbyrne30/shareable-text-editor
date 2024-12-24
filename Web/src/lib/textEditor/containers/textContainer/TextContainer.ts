import type { Cell } from '$lib/textEditor/types/Cell';
import type { ITextContainer } from '.';
import type { OnInputCallback, OnKeydownCallback, OnSelectionChangeCallback } from './types';

export interface TextContainerConfig {
	textarea: HTMLTextAreaElement;
}

export class TextContainer implements ITextContainer {
	private textarea: HTMLTextAreaElement;
	private isFocus = false;
	private onKeydownCb: OnKeydownCallback = () => {};
	private onSelectionChangeCb: OnSelectionChangeCallback = () => {};
	private onInputCb: OnInputCallback = () => {};

	constructor(config: TextContainerConfig) {
		this.textarea = config.textarea;
		this.setupEventListeners();
	}

	setupEventListeners = () => {
		document.addEventListener('keydown', this.onKeydown);
		this.textarea.addEventListener('selectionchange', this.onSelectionChange);
		this.textarea.addEventListener('input', this.onInput);
	};

	focus = () => {
		setTimeout(() => {
			this.textarea.focus();
			this.isFocus = true;
		}, 0);
	};

	onKeydown = (e: KeyboardEvent) => {
		if (!this.isFocus) return;
		this.onKeydownCb(e);
	};

	getText = (): string[] => {
		const text = this.textarea.value.split('\n');
		for (let i = 0; i < text.length - 1; i++) text[i] += '\n';
		return text;
	};

	charPosToCell = (pos: number): Cell => {
		const lines = this.getText();
		let count = pos;
		for (let i = 0; i < lines.length; i++) {
			const cols = lines[i].length;
			if (count < cols) return { row: i, col: count };
			count -= cols;
		}
		return { row: lines.length - 1, col: lines[lines.length - 1].length };
	};

	onSelectionChange = () => {
		this.textarea.selectionDirection;
		const start = this.charPosToCell(this.textarea.selectionStart);
		const end = this.charPosToCell(this.textarea.selectionEnd);
		this.onSelectionChangeCb({
			selectionStart: start,
			selectionEnd: end,
			direction: this.textarea.selectionDirection
		});
	};

	onInput = () => {
		this.onInputCb(this.textarea.value);
	};

	setOnKeyDown = (cb: OnKeydownCallback) => {
		this.onKeydownCb = cb;
	};

	setOnSelectionChange = (cb: OnSelectionChangeCallback) => {
		this.onSelectionChangeCb = cb;
	};

	setOnInput = (cb: OnInputCallback) => {
		this.onInputCb = cb;
	};

	cleanup = () => {
		document.removeEventListener('keydown', this.onKeydown);
		this.textarea.removeEventListener('selectionchange', this.onSelectionChange);
		this.textarea.removeEventListener('input', this.onInput);
	};
}
