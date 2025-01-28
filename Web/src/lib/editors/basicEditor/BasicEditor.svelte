<script lang="ts">
	import type { HTMLTextareaAttributes } from 'svelte/elements';
	import { EditorEvent } from '../../events/EditorEvent';
	import { InputHandler } from './InputHandler';
	import { BasicEditorSocket } from './services/BasicEditorSocket';
	import { onDestroy, onMount } from 'svelte';
	import { env } from '$env/dynamic/public';
	import { Dialog } from '@/lib/components/dialog';
	import { Yata } from '@/lib/algorithms/yata';

	let prevContent: string = '';
	let textarea: HTMLTextAreaElement | null = null;
	let {
		documentId,
		documentContent,
		...restprops
	}: HTMLTextareaAttributes & { documentId: string; documentContent: string } = $props();
	let basicEditorSocket: BasicEditorSocket | null = null;
	let isEditable = $state(false);
	const disconnectedTimeoutMs = 2000;
	let disconnectedTimeout: NodeJS.Timeout | number = 0;
	let showDisconnected = $state(false);
	const yata = new Yata([]);
	let replicaId = new Date().getTime();
	let yataOut = $state('');
	let sequence = $state(0);

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

	function onRemoteMessage(event: string) {
		if (!textarea) return;
		const remoteYata = Yata.parseJson(JSON.parse(event));
		yata.merge(remoteYata);
		yataOut = yata.toString();
	}

	function handleEvent(event: EditorEvent) {
		for (let i = 0; i < event.delete; i++) {
			yata.delete(event.pos);
		}
		for (let i = 0; i < event.insert.length; i++) {
			yata.insert(replicaId, event.pos + i, event.insert[i], sequence);
			sequence += 1;
		}
		yataOut = yata.toString();

		basicEditorSocket?.send(JSON.stringify(yata.toJson()));
	}

	function onSocketClose() {
		isEditable = false;
		basicEditorSocket = createEditor();
		disconnectedTimeout = setTimeout(() => {
			showDisconnected = true;
		}, disconnectedTimeoutMs);
	}

	function onSocketOpen() {
		clearTimeout(disconnectedTimeout);
		showDisconnected = false;
		isEditable = true;
	}

	function createEditor(): BasicEditorSocket {
		const editor = new BasicEditorSocket(
			env.PUBLIC_DOCUMENT_WEBSOCKET_URL + '/' + documentId + '/p2p'
		);
		editor.onMessage(onRemoteMessage);
		editor.onClose(onSocketClose);
		editor.onOpen(onSocketOpen);
		return editor;
	}

	onMount(() => {
		basicEditorSocket = createEditor();
	});

	onDestroy(() => {
		basicEditorSocket?.close();
	});
</script>

<article>
	<p>YATA output:</p>
	{yataOut}
</article>

<textarea
	bind:this={textarea}
	bind:value={yataOut}
	{...restprops}
	defaultValue={documentContent}
	readonly={!isEditable}
	{onbeforeinput}
	{oninput}
></textarea>

<Dialog open={showDisconnected}>
	<div class="rounded bg-background p-4">Disconnected from server. Try refreshing the page.</div>
</Dialog>
