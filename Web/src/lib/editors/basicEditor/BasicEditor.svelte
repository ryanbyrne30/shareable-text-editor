<script lang="ts">
	import type { HTMLTextareaAttributes } from 'svelte/elements';
	import { EditorEvent } from './EditorEvent';
	import { InputHandler } from './InputHandler';

	let prevContent: string = '';
	let textarea: HTMLTextAreaElement | null = null;
	let { ...restprops }: HTMLTextareaAttributes = $props();

	type TextAreaEvent<T> = T & { currentTarget: EventTarget & HTMLTextAreaElement };

	function oninput(evt: TextAreaEvent<Event>) {
		if (!textarea) return;
		const event = InputHandler.oninput(evt, prevContent, textarea);
		prevContent = evt.currentTarget.value;
		if (event !== null) handleEvent(event);
	}

	function onbeforeinput(e: TextAreaEvent<InputEvent>) {
		if (!textarea) return;
		const event = InputHandler.onbeforeinput(e, prevContent, textarea);
		if (event) handleEvent(event);
	}

	function handleEvent(event: EditorEvent) {
		console.log(event.toString());
	}
</script>

<textarea bind:this={textarea} {...restprops} {onbeforeinput} {oninput}></textarea>
