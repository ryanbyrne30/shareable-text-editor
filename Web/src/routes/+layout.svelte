<script lang="ts">
	import { QueryClient, QueryClientProvider } from '@sveltestack/svelte-query';
	import '../app.css';
	import { ModeWatcher } from 'mode-watcher';
	import { updateUserStore, userStore } from '@/usecases/common/client/stores/userStore';
	import { onMount } from 'svelte';
	import MainNav from './MainNav.svelte';
	let { children } = $props();

	const queryClient = new QueryClient();
	onMount(updateUserStore);
</script>

<ModeWatcher />
<QueryClientProvider client={queryClient}>
	<MainNav />
	{@render children()}
</QueryClientProvider>
