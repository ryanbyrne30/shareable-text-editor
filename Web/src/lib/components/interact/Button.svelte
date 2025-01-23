<script lang="ts">
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	let {
		children,
		class: className,
		isLoading = false,
		...restProps
	}: HTMLButtonAttributes & { isLoading?: boolean } = $props();
</script>

<button
	{...restProps}
	class={twMerge(
		'w-fit rounded border border-border bg-primary px-4 py-2 text-primary-foreground',
		isLoading ? 'animate-pulse' : '',
		className
	)}
>
	{#if children}
		{@render children()}
	{/if}
</button>
