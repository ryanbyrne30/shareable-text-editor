export class EditorEvent {
	public pos: number;
	public delete: number;
	public insert: string;
	private encoder = new TextEncoder();

	constructor(text: string | null, startpos: number, endpos: number) {
		this.pos = startpos;
		this.delete = endpos - startpos;
		this.insert = text ?? '';
	}

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
}
