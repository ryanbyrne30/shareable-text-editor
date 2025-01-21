<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { signOutUserRequest } from './signOutUserRequest';

	let { children, ...restProps }: HTMLButtonAttributes = $props();

	async function onclick() {
		await signOutUserRequest();
		location.reload();
	}
</script>

<Button {...restProps} {onclick}>
	{#if children}
		{@render children()}
	{/if}
</Button>
