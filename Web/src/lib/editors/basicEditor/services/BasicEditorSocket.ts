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
			if (e.data instanceof ArrayBuffer) {
				const bytes = new Uint8Array(e.data);
				console.debug('Socket message (bytes):', bytes);
			} else if (e.data instanceof Blob) {
				const reader = new FileReader();
				reader.onload = () => {
					const arrayBuffer = reader.result as ArrayBuffer;
					const bytes = new Uint8Array(arrayBuffer);
					console.debug('Socket message (blob as bytes):', bytes);
				};
				reader.readAsArrayBuffer(e.data);
			} else {
				console.debug('Socket message:', e.data);
			}
		});
	};

	public send = (message: string) => {
		this.socket.send(message);
	};

	public sendBytes = (message: ArrayBuffer) => {
		this.socket.send(message);
	};
}
