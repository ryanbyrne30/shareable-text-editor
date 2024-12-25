import { z } from 'zod';
import type { TextAction } from './TextAction';

const receivedMessageSchema = z.object({
	ack: z
		.object({
			success: z.boolean(),
			version: z.number()
		})
		.optional(),
	revision: z.number().nullish(),
	pos: z.number().nullish(),
	insert: z.string().nullish(),
	delete: z.number().nullish()
});
export type ReceivedMessage = z.infer<typeof receivedMessageSchema>;
export type OnMessageCallback = (msg: ReceivedMessage) => void;

export class SyncSocket {
	private socket: WebSocket;
	private onMessage: OnMessageCallback[] = [];

	constructor(endpoint: string) {
		this.socket = new WebSocket(endpoint);
		this.socket.addEventListener('message', this.#onMessage);
		this.socket.addEventListener('open', this.#onOpen);
		this.socket.addEventListener('close', this.#onClose);
	}

	sendChange = async (action: TextAction) => {
		this.socket.send(JSON.stringify(action));
	};

	addOnMessage = (cb: OnMessageCallback) => {
		this.onMessage.push(cb);
	};

	#onMessage = (event: MessageEvent<string>) => {
		const parsed = receivedMessageSchema.safeParse(JSON.parse(event.data));
		if (!parsed.success) {
			console.error('Received invalid response from server.');
			console.error(parsed.error);
			return;
		}
		for (let cb of this.onMessage) {
			cb(parsed.data);
		}
	};

	#onOpen = () => {
		console.log('Connected to socket');
	};

	#onClose = () => {
		console.log('Disconnected from socket');
	};
}
