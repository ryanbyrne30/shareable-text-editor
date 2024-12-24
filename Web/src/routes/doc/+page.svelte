<script lang="ts">
	import { NewEditor } from '$lib/textEditor';
	import type { EditorContainer } from '$lib/textEditor/containers/editorContainer/EditorContainer';

	const width = 500;
	const height = 500;
	let textarea: HTMLTextAreaElement | null = null;
	let textCanvas: HTMLCanvasElement | null = null;
	let cursorCanvas: HTMLCanvasElement | null = null;
	let editor: EditorContainer | null = null;

	function setup() {
		if (!textarea || !textCanvas || !cursorCanvas) return;
		editor = NewEditor({
			textarea,
			textCanvas,
			cursorCanvas
		});
	}

	$: textarea && textCanvas && cursorCanvas && setup();
</script>

<textarea bind:this={textarea} class="border-2 font-mono"></textarea>
<div class="relative">
	<canvas bind:this={cursorCanvas} {width} {height} class="absolute left-0 top-0 border font-mono"
	></canvas>
	<canvas bind:this={textCanvas} {width} {height} class="absolute left-0 top-0 font-mono"></canvas>
</div>
