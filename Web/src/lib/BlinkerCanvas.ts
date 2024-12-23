import type { Layout } from './Layout';

export interface BlinkerCanvasConfig {
	layout: Layout;
	context: CanvasRenderingContext2D;
	timeoutMs: number;
	height: number;
	width: number;
	cellRow: number;
	cellCol: number;
	fill?: string;
}

export class BlinkerCanvas {
	private layout: Layout;
	private context: CanvasRenderingContext2D;
	private timeoutMs: number;
	private height: number;
	private width: number;
	private cellRow: number;
	private cellCol: number;
	private fill: string;
	private timeout: number = 0;

	constructor(config: BlinkerCanvasConfig) {
		this.layout = config.layout;
		this.context = config.context;
		this.timeoutMs = config.timeoutMs;
		this.height = config.height;
		this.width = config.width;
		this.cellRow = config.cellRow;
		this.cellCol = config.cellCol;
		this.fill = config.fill ?? 'gray';
	}

	start = () => {
		this.#draw();
	};

	stop = () => {
		clearTimeout(this.timeout);
		this.#clear();
	};

	#draw = () => {
		clearTimeout(this.timeout);
		this.context.fillStyle = this.fill;
		const { x, y } = this.layout.cellToCanvasCoord(this.cellRow, this.cellCol);
		this.context.fillRect(x, y, this.width, this.height);
		this.timeout = setTimeout(this.#clear, this.timeoutMs);
	};

	#clear = () => {
		clearTimeout(this.timeout);
		const { x, y } = this.layout.cellToCanvasCoord(this.cellRow, this.cellCol);
		this.context.clearRect(x, y, this.width, this.height);
		this.timeout = setTimeout(this.#draw, this.timeoutMs);
	};

	move = (cellRow: number, cellCol: number) => {
		this.stop();
		this.cellRow = cellRow;
		this.cellCol = cellCol;
	};
}
