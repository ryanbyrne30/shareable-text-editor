import { Canvas } from './components/canvas/Canvas';
import { Grid } from './components/grid/Grid';
import { TextContainer } from './containers/textContainer/TextContainer';
import { EditorContainer } from './containers/editorContainer/EditorContainer';
import { CursorCanvas } from './containers/cursorCanvas/CursorCanvas';
import { TextCanvas } from './containers/textCanvas/TextCanvas';

export function NewEditor(args: {
	textarea: HTMLTextAreaElement;
	textCanvas: HTMLCanvasElement;
	cursorCanvas: HTMLCanvasElement;
	paddingX?: number;
	paddingY?: number;
}): EditorContainer {
	const grid = new Grid({
		canvas: args.textCanvas,
		canvasPaddingX: args.paddingX,
		canvasPaddingY: args.paddingY,
		colWidth: 12,
		rowHeight: 16,
		rowPadding: 4
	});

	const cursorCanvas = new CursorCanvas({
		canvas: new Canvas(args.cursorCanvas),
		grid
	});

	const textCanvas = new TextCanvas({
		canvas: new Canvas(args.textCanvas),
		grid
	});

	const textContainer = new TextContainer({
		textarea: args.textarea
	});

	return new EditorContainer({
		cursorCanvas,
		textCanvas,
		textContainer
	});
}
