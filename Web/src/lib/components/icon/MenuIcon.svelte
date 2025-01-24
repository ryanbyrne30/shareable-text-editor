<script lang="ts">
	import type { HTMLAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';

	let { class: className, ...restProps }: HTMLAttributes<SVGElement> = $props();
</script>

<svg
	{...restProps}
	class={twMerge('h-6 w-6 fill-foreground stroke-foreground', className)}
	viewBox="0 0 24.00 24.00"
	fill="none"
	xmlns="http://www.w3.org/2000/svg"
	><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g
		id="SVGRepo_tracerCarrier"
		stroke-linecap="round"
		stroke-linejoin="round"
		stroke-width="0.048"
	></g><g id="SVGRepo_iconCarrier">
		<path
			d="M4 6H20M4 12H20M4 18H20"
			stroke-width="2.4"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</g></svg
>
