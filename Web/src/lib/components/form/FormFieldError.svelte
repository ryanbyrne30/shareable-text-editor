<script lang="ts">
	import type { HTMLAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let { class: className, children, ...restProps }: HTMLAttributes<HTMLParagraphElement> = $props();
</script>

{#if children}
	<p {...restProps} class={twMerge('text-error', className)}>
		{@render children()}
	</p>
{/if}
