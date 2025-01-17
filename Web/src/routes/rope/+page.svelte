<script lang="ts">
	import { EditorCanvas } from '$lib/editorCanvas';
	import { onMount } from 'svelte';

	const width = 500;
	const height = 500;
	let textCanvasEl: HTMLCanvasElement | null = null;
	let cursorCanvasEl: HTMLCanvasElement | null = null;

	function setup() {
		if (textCanvasEl === null || cursorCanvasEl === null) return;
		new EditorCanvas(height, width, textCanvasEl, cursorCanvasEl);
	}

	onMount(setup);
</script>

<main class="flex h-screen w-screen flex-col items-center justify-center">
	<div class="relative h-[90dvh] w-[90dvw] overflow-x-scroll overflow-y-scroll bg-gray-200">
		<canvas
			bind:this={textCanvasEl}
			{width}
			{height}
			class="absolute left-0 top-0 border border-black"
		></canvas>
		<canvas bind:this={cursorCanvasEl} {width} {height} class="absolute left-0 top-0"></canvas>
	</div>
</main>
