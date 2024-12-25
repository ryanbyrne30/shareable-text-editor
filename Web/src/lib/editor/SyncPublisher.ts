import type { SyncSocket } from './SyncSocket';
import type { TextAction } from './TextAction';

export class SyncPublisher {
	private queue: TextAction[] = [];
	private mutex = false;

	constructor(private socket: SyncSocket) {}

	publish = async (action: TextAction) => {
		this.queue.unshift(action);
		if (this.mutex) return;
		this.mutex = true;
		await this.#sync();
	};

	#sync = async () => {
		const action = this.queue.pop();
		if (!action) {
			this.mutex = false;
			return;
		}
		await this.socket.send(action.toString());
		this.#sync();
	};
}
