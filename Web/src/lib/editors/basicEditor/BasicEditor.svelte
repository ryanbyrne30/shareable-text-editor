<script lang="ts">
	import type { HTMLTextareaAttributes } from 'svelte/elements';
	import { EditorEvent } from './EditorEvent';
	import { InputHandler } from './InputHandler';
	import { BasicEditorSocket } from './services/BasicEditorSocket';
	import { onMount } from 'svelte';
	import { env } from '$env/dynamic/public';

	let prevContent: string = '';
	let textarea: HTMLTextAreaElement | null = null;
	let { documentId, ...restprops }: HTMLTextareaAttributes & { documentId: string } = $props();
	let basicEditorSocket: BasicEditorSocket | null = null;

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
		basicEditorSocket?.sendBytes(event.toBinary());
		console.debug(event.toString());
	}

	onMount(() => {
		basicEditorSocket = new BasicEditorSocket(env.PUBLIC_DOCUMENT_WEBSOCKET_URL + '/' + documentId);
	});
</script>

<textarea bind:this={textarea} {...restprops} {onbeforeinput} {oninput}></textarea>
