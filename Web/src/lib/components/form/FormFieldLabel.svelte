<script lang="ts">
	import type { HTMLLabelAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let { class: className, children, ...restProps }: HTMLLabelAttributes = $props();
</script>

<label {...restProps} class={twMerge('flex flex-col gap-1', className)}>
	{#if children}
		{@render children()}
	{/if}
</label>
