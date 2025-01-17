export type CursorMoveEvent = { x: number; y: number };
export type CursorMoveEventListener = (event: CursorMoveEvent) => void;

export type CanvasResizeEvent = { w: number; h: number };
export type CanvasResizeEventListener = (event: CanvasResizeEvent) => void;

export class EventManager {
	private static cursorMoveEventListeners: CursorMoveEventListener[] = [];
	private static canvasResizeEventListeners: CanvasResizeEventListener[] = [];

	public static addCanvasResizeEventListener = (handler: CanvasResizeEventListener) => {
		this.canvasResizeEventListeners.push(handler);
	};

	public static emitCanvasResizeEvent = (event: CanvasResizeEvent) => {
		for (let h of this.canvasResizeEventListeners) {
			h(event);
		}
	};

	public static addCursorMoveEventListener = (handler: CursorMoveEventListener) => {
		this.cursorMoveEventListeners.push(handler);
	};

	public static emitCursorMoveEvent = (event: CursorMoveEvent) => {
		for (let h of this.cursorMoveEventListeners) {
			h(event);
		}
	};
}
