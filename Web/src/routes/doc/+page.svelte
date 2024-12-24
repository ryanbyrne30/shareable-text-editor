<script lang="ts">
	import { TextContainer } from '$lib/editor/TextContainer';
	import { onMount } from 'svelte';
	import { twMerge } from 'tailwind-merge';

	let textarea: HTMLTextAreaElement | null = null;
	let editorCanvas: HTMLCanvasElement | null = null;
	let blinkerCanvas: HTMLCanvasElement;

	function setup() {
		if (!editorCanvas || !blinkerCanvas || !textarea) return;
		const textContainer = new TextContainer(textarea);
	}

	onMount(() => {
		setup();
	});
</script>

<p>Type below</p>
<textarea bind:this={textarea} class="border"></textarea>
<!-- svelte-ignore a11y_no_noninteractive_tabindex -->
<div tabindex="0" class="relative border bg-gray-100">
	<canvas
		bind:this={editorCanvas}
		width="500"
		height="500"
		class={twMerge('absolute left-0 top-0 border-2')}
	></canvas>
	<canvas bind:this={blinkerCanvas} width="500" height="500" class="absolute left-0 top-0"></canvas>
</div>
