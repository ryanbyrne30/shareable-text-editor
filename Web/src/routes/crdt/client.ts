import { CharacterKeys, KeyboardKey } from './keys';

export type ClientState = {
	isFocused: boolean;
};

export type OnKeyDownHandler = (key: string) => void;

export class ClientEventDispatcher {
	private static clients: ClientState[] = [
		{ isFocused: false },
		{ isFocused: false },
		{ isFocused: false }
	];
	private static onKeyDownHandlers: OnKeyDownHandler[] = [];

	constructor() {
		document.addEventListener('keydown', ClientEventDispatcher.emitKeyDownEvent);
	}

	public static addKeyDownHandler = (handler: OnKeyDownHandler) => {
		this.onKeyDownHandlers.push(handler);
	};

	public static getClients = (): ClientState[] => {
		return this.clients;
	};

	public static changeClientState = (index: number, state: ClientState) => {
		this.clients[index] = { ...state };
	};

	private static emitKeyDownEvent = (e: KeyboardEvent) => {
		const { index } = this.getActiveClient();
		const h = this.onKeyDownHandlers[index];
		if (h !== undefined) h(e.key);
	};

	private static getActiveClient = () => {
		const index = this.clients.findIndex((c) => c.isFocused);
		return {
			index,
			client: this.clients[index]
		};
	};
}

export class ActionBuilder {
	private debounceMs: number = 1000;
	private keysPressed: string[] = [];
	private text: string = '';
	private cursor: number = 0;
	private timeout: number = 0;

	public addKey = (key: string) => {
		clearTimeout(this.timeout);
		this.keysPressed.unshift(key);
		this.timeout = setTimeout(this.resolveKeys, this.debounceMs);
	};

	public setCursor = (pos: number) => {
		console.log('Setting cursor to pos:', pos);
		if (pos < 0) this.cursor = 0;
		else if (pos > this.text.length) pos = this.text.length;
		else this.cursor = pos;
	};

	private enumHasValue = (e: object, value: string): boolean => {
		return Object.values(e).includes(value);
	};

	private insertAt = (index: number, char: string) => {
		if (index >= this.text.length) this.text += char;
		if (index < 0) this.text = char + this.text;
		else this.text.slice(0, index) + char + this.text.slice(index);
	};

	private deleteAt = (index: number) => {
		if (index < this.text.length && index >= 0)
			this.text = this.text.slice(0, index) + this.text.slice(index + 1);
	};

	private insertAtCursor = (char: string) => {
		this.insertAt(this.cursor, char);
		this.cursor = this.cursor += 1;
	};

	private resolveKeys = () => {
		while (this.keysPressed.length > 0) {
			const key = this.keysPressed.pop();
			if (key === undefined) break;
			if (this.enumHasValue(CharacterKeys, key)) this.insertAtCursor(key);
			if (KeyboardKey.Backspace === key) this.deleteAt(this.cursor);
			if (KeyboardKey.Delete === key) this.deleteAt(this.cursor);
			if (KeyboardKey.Enter === key) this.insertAtCursor('\n');
		}
		console.log('Text:', this.text);
	};
}
