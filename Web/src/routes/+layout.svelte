<script lang="ts">
	import { QueryClient, QueryClientProvider } from '@sveltestack/svelte-query';
	import '../app.css';
	import { ModeWatcher } from 'mode-watcher';
	let { children } = $props();

	const queryClient = new QueryClient();
</script>

<ModeWatcher />
<QueryClientProvider client={queryClient}>
	{@render children()}
</QueryClientProvider>
