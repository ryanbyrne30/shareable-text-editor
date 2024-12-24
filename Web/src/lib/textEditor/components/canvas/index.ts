export interface ICanvas {
	drawRect(
		x: number,
		y: number,
		width: number,
		height: number,
		opts?: {
			style?: string;
		}
	): void;
	drawChar(char: string, x: number, y: number, opts?: { style?: string; size?: number }): void;
	clearRect(x: number, y: number, width: number, height: number, opts?: { style?: string }): void;
	clear(): void;
}
