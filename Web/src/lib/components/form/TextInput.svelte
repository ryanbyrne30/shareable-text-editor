<script lang="ts">
	import type { HTMLInputAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let { class: className, ...restProps }: HTMLInputAttributes = $props();
</script>

<input {...restProps} class={twMerge('rounded border px-2 py-1', className)} />
