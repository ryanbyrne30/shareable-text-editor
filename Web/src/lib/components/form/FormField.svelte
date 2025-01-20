<script lang="ts">
	import type { HTMLAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	let { children, class: className, ...restProps }: HTMLAttributes<HTMLDivElement> = $props();
</script>

<div {...restProps} class={twMerge('flex flex-col gap-2', className)}>
	{#if children}
		{@render children()}
	{/if}
</div>
