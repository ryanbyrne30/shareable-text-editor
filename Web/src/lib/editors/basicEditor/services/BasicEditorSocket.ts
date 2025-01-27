import { EditorEvent } from '../../../events/EditorEvent';

type OnMessageCallback = (event: EditorEvent) => void;
type Callback = () => void;

export class BasicEditorSocket {
	private socket: WebSocket;
	private manuallyClosed = false;
	private onMessageCallbacks: OnMessageCallback[] = [];
	private onOpenCallbacks: Callback[] = [];
	private onCloseCallbacks: Callback[] = [];

	constructor(private webSocketUrl: string) {
		this.socket = new WebSocket(webSocketUrl);
		this.setup();
	}

	private setup = () => {
		this.socket.addEventListener('open', () => {
			console.debug('Socket connected');
			this.onOpenCallbacks.forEach((cb) => cb());
		});
		this.socket.addEventListener('close', () => {
			console.debug('Socket disconnected');
			if (this.manuallyClosed) return;
			this.onCloseCallbacks.forEach((cb) => cb());
		});
		this.socket.addEventListener('error', (e) => {
			console.debug('Socket error');
			console.error(e);
		});
		this.socket.addEventListener('message', (e) => {
			this.handleMessage(e.data);
		});
	};

	public send = (message: string) => {
		this.socket.send(message);
	};

	public sendBytes = (message: ArrayBuffer) => {
		this.socket.send(message);
	};

	public onMessage = (cb: OnMessageCallback) => {
		this.onMessageCallbacks.push(cb);
	};

	public onOpen = (cb: Callback) => {
		this.onOpenCallbacks.push(cb);
	};

	public onClose = (cb: Callback) => {
		this.onCloseCallbacks.push(cb);
	};

	private handleMessage = (message: any) => {
		if (message instanceof ArrayBuffer) {
			this.handleArrayBufferMessage(message);
		} else if (message instanceof Blob) {
			const reader = new FileReader();
			reader.onload = () => {
				const arrayBuffer = reader.result as ArrayBuffer;
				this.handleArrayBufferMessage(arrayBuffer);
			};
			reader.readAsArrayBuffer(message);
		} else {
			console.error('(unhandled) Socket message:', message);
		}
	};

	private handleArrayBufferMessage = (message: ArrayBuffer) => {
		const event = EditorEvent.parseBinary(message);
		this.onMessageCallbacks.forEach((cb) => {
			cb(event);
		});
	};

	public close = () => {
		this.manuallyClosed = true;
		this.socket.close();
	};
}
