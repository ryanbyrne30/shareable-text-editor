<script lang="ts">
	import type { HTMLFormAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	let { children, class: className, ...restProps }: HTMLFormAttributes = $props();
</script>

<form {...restProps} class={twMerge('flex flex-col gap-8', className)}>
	{#if children}
		{@render children()}
	{/if}
</form>
