import { EditorEvent } from '../../events/EditorEvent';
import { RawInputEventType } from './RawInputEventType';

type TextAreaEvent<T> = T & { currentTarget: EventTarget & HTMLTextAreaElement };

export class InputHandler {
	public static oninput = (
		evt: TextAreaEvent<Event>,
		prevContent: string,
		textarea: HTMLTextAreaElement
	): EditorEvent | null => {
		const e = evt as any as TextAreaEvent<InputEvent>;

		if (
			e.inputType !== RawInputEventType.DeleteWordBackward &&
			e.inputType !== RawInputEventType.DeleteWordForward
		) {
			return null;
		}

		const text = e.data;
		const startpos = textarea?.selectionStart ?? 0;
		const curText = e.currentTarget.value;
		const endpos = startpos + prevContent.length - curText.length;

		return EditorEvent.newFromInputEvent(text, startpos, endpos);
	};

	public static onbeforeinput = (
		e: TextAreaEvent<InputEvent>,
		prevContent: string,
		textarea: HTMLTextAreaElement
	): EditorEvent | null => {
		if (
			e.inputType === RawInputEventType.HistoryUndo ||
			e.inputType === RawInputEventType.HistoryRedo
		) {
			e.preventDefault();
			return null;
		}
		if (
			e.inputType === RawInputEventType.DeleteWordBackward ||
			e.inputType === RawInputEventType.DeleteWordForward
		)
			return null;

		let text = e.data;
		let startpos = textarea?.selectionStart ?? 0;
		let endpos = textarea?.selectionEnd ?? 0;

		if (e.inputType === RawInputEventType.InsertLineBreak) text = '\n';

		if (e.inputType === RawInputEventType.DeleteContentBackward && startpos === endpos) {
			startpos = Math.max(0, startpos - 1);
		}
		if (e.inputType === RawInputEventType.DeleteContentForward && startpos === endpos) {
			endpos = Math.min(prevContent.length, endpos + 1);
		}

		return EditorEvent.newFromInputEvent(text, startpos, endpos);
	};
}
