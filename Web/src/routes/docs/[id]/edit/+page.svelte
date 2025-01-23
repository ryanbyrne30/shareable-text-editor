<script lang="ts">
	import { marked } from 'marked';
	import { onMount } from 'svelte';
	import { twMerge } from 'tailwind-merge';

	let textarea: HTMLTextAreaElement | null = null;
	let content: HTMLDivElement | null = null;
	let text: string = '';
	let showPreview = false;
	let orientation: 'horizontal' | 'vertical' = 'vertical';
	let disableContentScroll = false;
	let disableTextScroll = false;
	let disableContentScrollTimeout = 0;
	let disableTextScrollTimeout = 0;
	let scrollLockTimeoutMs = 500;

	function render() {
		if (content === null) return;
		content.innerHTML = marked.parse(text) as string;
	}

	function syncScroll(fromEl: HTMLElement, toEl: HTMLElement) {
		const yprop = getScrollYProportion(fromEl);
		const xprop = getScrollXProportion(fromEl);
		const { height, width } = toEl.getBoundingClientRect();
		toEl.scrollTop = yprop * (toEl.scrollHeight - height);
		toEl.scrollLeft = xprop * (toEl.scrollWidth - width);
	}

	function getScrollYProportion(el: HTMLElement) {
		const scroll = el.scrollTop;
		const height = el.scrollHeight - el.getBoundingClientRect().height;
		return scroll / height;
	}

	function getScrollXProportion(el: HTMLElement) {
		const scroll = el.scrollLeft;
		const width = el.scrollWidth - el.getBoundingClientRect().width;
		return scroll / width;
	}

	$: text !== undefined && render();

	onMount(() => {
		if (!textarea || !content) return;
		textarea.addEventListener('scroll', function () {
			if (!content || !textarea || disableTextScroll) return;
			clearTimeout(disableContentScrollTimeout);
			disableContentScroll = true;
			syncScroll(textarea, content);
			disableContentScrollTimeout = setTimeout(() => {
				disableContentScroll = false;
			}, scrollLockTimeoutMs);
		});

		content.addEventListener('scroll', function () {
			if (!content || !textarea || disableContentScroll) return;
			clearTimeout(disableTextScrollTimeout);
			disableTextScroll = true;
			syncScroll(content, textarea);
			disableTextScrollTimeout = setTimeout(() => {
				disableTextScroll = false;
			}, scrollLockTimeoutMs);
		});
	});
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
		'grid h-[calc(100dvh-72px)]',
		orientation === 'horizontal' ? 'grid-cols-1 grid-rows-2' : 'grid-cols-2 grid-rows-1',
		!showPreview ? 'grid-cols-1 grid-rows-1' : ''
	)}
>
	<textarea
		bind:this={textarea}
		bind:value={text}
		class={twMerge('h-full w-full resize-none border border-black p-4 font-mono')}
	></textarea>
	<div
		bind:this={content}
		class={twMerge('overflow-scroll p-4', showPreview ? '' : 'hidden')}
	></div>
</main>
