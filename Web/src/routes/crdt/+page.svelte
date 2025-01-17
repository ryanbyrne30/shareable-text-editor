<script lang="ts">
	import { onMount } from 'svelte';
	import { ClientEventDispatcher, type ClientState } from './client';
	import ClientUi from './client-ui.svelte';

	onMount(() => {
		new ClientEventDispatcher();
	});
</script>

<h1>CRDT</h1>

<section class="flex flex-col gap-8 p-8">
	{#each ClientEventDispatcher.getClients() as c, i (i)}
		<ClientUi index={i} state={c} />
	{/each}
</section>
