<script lang="ts">
	import { marked } from 'marked';
	import { twMerge } from 'tailwind-merge';

	let content: HTMLDivElement | null = null;
	let text: string = '';
	let showPreview = false;
	let orientation: 'horizontal' | 'vertical' = 'vertical';

	function render() {
		if (content === null) return;
		content.innerHTML = marked.parse(text) as string;
	}

	const contentConfig = {
		horizontal: 'h-1/2 w-full',
		vertical: 'w-1/2 h-full',
		default: 'hidden'
	};

	function getContentConfig(showPreview: boolean, orientation: string) {
		if (!showPreview) return contentConfig.default;
		if (orientation === 'horizontal') return contentConfig.horizontal;
		return contentConfig.vertical;
	}

	$: text !== undefined && render();
</script>

<div class="flex flex-row gap-4 p-4">
	<button onclick={() => (showPreview = !showPreview)} class="rounded-md bg-blue-300 px-4 py-2"
		>{showPreview ? 'Hide' : 'Show'} preview</button
	>
	<button
		onclick={() => (orientation = orientation === 'horizontal' ? 'vertical' : 'horizontal')}
		class="rounded-md bg-blue-300 px-4 py-2">Orientation</button
	>
</div>
<main
	class={twMerge(
		'grid h-[calc(100dvh-6rem)]',
		orientation === 'horizontal' ? 'grid-cols-1 grid-rows-2' : 'grid-cols-2 grid-rows-1',
		!showPreview ? 'grid-cols-1 grid-rows-1' : ''
	)}
>
	<textarea
		bind:value={text}
		class={twMerge('h-full w-full resize-none border border-black p-4 font-mono')}
	></textarea>
	<div
		bind:this={content}
		class={twMerge('overflow-scroll p-4', showPreview ? '' : 'hidden')}
	></div>
</main>
