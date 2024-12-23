<script lang="ts">
	import { Editor } from '$lib/Editor';
	import { onMount } from 'svelte';
	import { twMerge } from 'tailwind-merge';

	let input = '';
	let textarea: HTMLTextAreaElement | null = null;
	let editorCanvas: HTMLCanvasElement | null = null;
	let blinkerCanvas: HTMLCanvasElement;
	let editor: Editor | null = null;
	let isFocus = false;

	function setup() {
		if (!editorCanvas || !blinkerCanvas || !textarea) return;
		editor = new Editor({
			textarea,
			editorCanvas,
			blinkerCanvas
		});
		editor.writeText(input);
		textarea.addEventListener('focus', () => {
			isFocus = document.activeElement === textarea;
		});
		document.addEventListener('click', (e) => {
			if (!blinkerCanvas.contains(e.target as any)) isFocus = false;
		});
	}

	function writeText(text: string) {
		if (!editor) return;
		editor.writeText(text);
	}

	$: writeText(input);

	onMount(() => {
		setup();
	});
</script>

<p>Type below</p>
<textarea bind:this={textarea} bind:value={input} class="absolute border-2 font-mono opacity-0"
></textarea>
<!-- svelte-ignore a11y_no_noninteractive_tabindex -->
<div tabindex="0" class="relative border bg-gray-100">
	<canvas
		bind:this={editorCanvas}
		width="500"
		height="500"
		class={twMerge('absolute left-0 top-0 border-2', isFocus ? 'border-red-500' : '')}
	></canvas>
	<canvas bind:this={blinkerCanvas} width="500" height="500" class="absolute left-0 top-0"></canvas>
</div>
