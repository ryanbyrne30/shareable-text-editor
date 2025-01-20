<script lang="ts">
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	let { children, class: className, ...restProps }: HTMLButtonAttributes = $props();
</script>

<button
	{...restProps}
	class={twMerge(
		'w-fit rounded border border-border bg-primary px-4 py-2 text-primary-foreground',
		className
	)}
>
	{#if children}
		{@render children()}
	{/if}
</button>
