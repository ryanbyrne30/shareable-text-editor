<script lang="ts">
	import type { HTMLInputAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let {
		me = $bindable(),
		class: className,
		value = $bindable(),
		...restProps
	}: HTMLInputAttributes & { me: HTMLInputElement | null } = $props();
</script>

<input
	bind:this={me}
	bind:value
	{...restProps}
	class={twMerge('rounded border px-2 py-1', className)}
/>
