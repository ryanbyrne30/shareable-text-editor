<script lang="ts">
	import type { HTMLTextareaAttributes } from 'svelte/elements';
	import { EditorEvent } from './EditorEvent';
	import { RawInputEventType } from './RawInputEventType';

	let prevcontent: string = '';
	let textarea: HTMLTextAreaElement | null = null;
	let { ...restprops }: HTMLTextareaAttributes = $props();

	type TextAreaEvent<T> = T & { currentTarget: EventTarget & HTMLTextAreaElement };

	function oninput(et: TextAreaEvent<Event>) {
		const e = et as any as TextAreaEvent<InputEvent>;

		if (
			e.inputType !== RawInputEventType.DeleteWordBackward &&
			e.inputType !== RawInputEventType.DeleteWordForward
		) {
			prevcontent = e.currentTarget.value;
			return;
		}

		const text = e.data;
		const startpos = textarea?.selectionStart ?? 0;
		const curText = e.currentTarget.value;
		const endpos = startpos + prevcontent.length - curText.length;

		const event = new EditorEvent(text, startpos, endpos);

		prevcontent = e.currentTarget.value;
	}

	function onbeforeinput(e: TextAreaEvent<InputEvent>) {
		if (
			e.inputType === RawInputEventType.HistoryUndo ||
			e.inputType === RawInputEventType.HistoryRedo
		) {
			e.preventDefault();
			return;
		}
		if (
			e.inputType === RawInputEventType.DeleteWordBackward ||
			e.inputType === RawInputEventType.DeleteWordForward
		)
			return;

		let text = e.data;
		let startpos = textarea?.selectionStart ?? 0;
		const endpos = textarea?.selectionEnd ?? 0;

		if (e.inputType === RawInputEventType.InsertLineBreak) text = '\n';

		const event = new EditorEvent(text, startpos, endpos);
	}
</script>

<textarea bind:this={textarea} {...restprops} {onbeforeinput} {oninput}></textarea>
