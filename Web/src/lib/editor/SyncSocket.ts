export class SyncSocket {
	private socket: WebSocket;

	constructor(endpoint: string) {
		this.socket = new WebSocket(endpoint);
		this.socket.addEventListener('message', this.#onMessage);
		this.socket.addEventListener('open', this.#onOpen);
		this.socket.addEventListener('close', this.#onClose);
	}

	send = async (message: string) => {
		this.socket.send(message);
	};

	#onMessage = (event: MessageEvent<string>) => {
		console.log(event.data);
	};

	#onOpen = () => {
		console.log('Connected to socket');
	};

	#onClose = () => {
		console.log('Disconnected from socket');
	};
}
