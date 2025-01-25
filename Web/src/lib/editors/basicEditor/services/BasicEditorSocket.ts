import { EditorEvent } from '../EditorEvent';

export class BasicEditorSocket {
	private socket: WebSocket;

	constructor(webSocketUrl: string) {
		this.socket = new WebSocket(webSocketUrl);
		this.setup();
	}

	private setup = () => {
		this.socket.addEventListener('open', () => {
			console.debug('Socket connected');
		});
		this.socket.addEventListener('close', () => {
			console.debug('Socket disconnected');
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
			console.debug('Socket message:', message);
		}
	};

	private handleArrayBufferMessage = (message: ArrayBuffer) => {
		const event = EditorEvent.parseBinary(message);
		console.debug(event.toString());
	};
}
