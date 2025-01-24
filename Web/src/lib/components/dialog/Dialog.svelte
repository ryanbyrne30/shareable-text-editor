<script lang="ts">
	import type { HTMLDialogAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let { class: className, open, children, ...restprops }: HTMLDialogAttributes = $props();
</script>

<dialog
	{...restprops}
	{open}
	class={twMerge(
		'fixed bottom-0 left-0 right-0 top-0 z-50 flex h-dvh w-dvw flex-col items-center justify-center bg-transparent backdrop-blur-sm backdrop-brightness-75',
		className,
		!open ? 'hidden' : ''
	)}
>
	{#if children}
		{@render children()}
	{/if}
</dialog>
