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
			console.debug('Socket message:', e.data);
		});
	};

	public send = (message: string) => {
		this.socket.send(message);
	};
}
