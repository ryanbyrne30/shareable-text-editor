<script lang="ts">
	import { QueryClient, QueryClientProvider } from '@sveltestack/svelte-query';
	import '../app.css';
	import { ModeWatcher } from 'mode-watcher';
	import { updateUserStore, userStore } from '@/usecases/common/client/stores/userStore';
	import { onMount } from 'svelte';
	import { Button } from '@/lib/components/interact';
	import SignOutUserButton from '@/usecases/signOutUser/client/SignOutUserButton.svelte';
	let { children } = $props();

	const queryClient = new QueryClient();
	onMount(updateUserStore);
</script>

<ModeWatcher />
<QueryClientProvider client={queryClient}>
	<nav class="flex flex-row gap-4 px-4 py-2">
		{#if $userStore === null}
			<a href="/auth/sign-in">
				<Button>Sign In</Button>
			</a>
			<a href="/auth/register">
				<Button>Register</Button>
			</a>
		{:else}
			<SignOutUserButton>Sign Out</SignOutUserButton>
		{/if}
	</nav>
	{@render children()}
</QueryClientProvider>
