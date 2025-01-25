export class EditorEvent {
	public pos: number = 0;
	public delete: number = 0;
	public insert: string = '';
	private encoder = new TextEncoder();

	public static newFromInputEvent = (
		text: string | null,
		startpos: number,
		endpos: number
	): EditorEvent => {
		const event = new EditorEvent();
		event.pos = startpos;
		event.delete = endpos - startpos;
		event.insert = text ?? '';
		return event;
	};

	public toString = (): string => {
		return `InputEvent(pos: ${this.pos}, delete: ${this.delete}, insert: ${this.insert})`;
	};

	/**
	 * Creates a binary array with the following format:
	 * | Position (4 bytes) | Delete (4 bytes) | Insert Length (4 bytes) | Insert Data (variable length) |
	 */
	public toBinary = (): ArrayBuffer => {
		const insertBytes = this.encoder.encode(this.insert);
		const buffer = new ArrayBuffer(12 + insertBytes.length);
		const view = new DataView(buffer);

		view.setUint32(0, this.pos);
		view.setUint32(4, this.delete);
		view.setUint32(8, insertBytes.length);
		new Uint8Array(buffer, 12).set(insertBytes);

		return buffer;
	};

	public static parseBinary = (buffer: ArrayBuffer): EditorEvent => {
		const decoder = new TextDecoder();

		const pos = new DataView(buffer, 0, 4).getUint32(0);
		const del = new DataView(buffer, 4, 4).getUint32(0);
		const textLength = new DataView(buffer, 8, 4).getUint32(0);
		const textSlice = buffer.slice(12);
		const text = textLength > 0 ? decoder.decode(textSlice) : '';

		const event = new EditorEvent();
		event.pos = pos;
		event.delete = del;
		event.insert = text;

		return event;
	};
}
