<script lang="ts">
	import { twMerge } from 'tailwind-merge';
	import type { HTMLAttributes } from 'svelte/elements';

	let { class: className, children, ...restProps }: HTMLAttributes<HTMLDivElement> = $props();
</script>

<div
	{...restProps}
	class={twMerge(
		'w-fit rounded-xl border border-border bg-card p-8 text-card-foreground',
		className
	)}
>
	{#if children}
		{@render children()}
	{/if}
</div>
