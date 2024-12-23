<script lang="ts">
	import { onMount } from 'svelte';

	let input = '';
	let websocket: WebSocket | null = null;
	let messages: string[] = [];

	function submit() {
		if (websocket !== null) {
			websocket.send(input);
		}
		input = '';
	}

	onMount(() => {
		websocket = new WebSocket('ws://localhost:5000/ws');
		websocket.addEventListener('open', () => {
			console.log('Connected to chat');
		});
		websocket.addEventListener('message', (event) => {
			messages = [...messages, event.data];
		});
	});
</script>

<form on:submit={submit} class="flex flex-col">
	<label class="flex flex-col">
		Input
		<input bind:value={input} class="border" />
	</label>
	<button type="submit">Submit</button>
</form>

<p>Messages</p>
<ul class="flex flex-col gap-2">
	{#each messages as m}
		<li>{m}</li>
	{/each}
</ul>
