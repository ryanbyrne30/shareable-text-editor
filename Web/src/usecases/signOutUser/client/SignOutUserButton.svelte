<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { signOutUserRequest } from './signOutUserRequest';
	import { userStore } from '@/usecases/common/client/stores';

	let { children, ...restProps }: HTMLButtonAttributes = $props();

	async function onclick() {
		await signOutUserRequest();
		userStore.set(null);
		location.reload();
	}
</script>

<Button {...restProps} {onclick}>
	{#if children}
		{@render children()}
	{/if}
</Button>
