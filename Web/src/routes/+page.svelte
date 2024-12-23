<script lang="ts">
	let chatId = '';
	let input = '';
	let websocket: WebSocket | null = null;
	let messages: string[] = [];

	function submit() {
		if (websocket !== null) {
			websocket.send(input);
			input = '';
		}
	}

	function onSocketOpen() {
		console.log('Connected to chat');
	}

	function onSocketMessage(event: MessageEvent<string>) {
		messages = [...messages, event.data];
	}

	function createSocket(chatId: string) {
		websocket = new WebSocket('ws://localhost:5000/ws/' + chatId);
		websocket.addEventListener('open', onSocketOpen);
		websocket.addEventListener('message', onSocketMessage);
	}

	function cleanupSocket() {
		websocket?.removeEventListener('open', onSocketOpen);
		websocket?.removeEventListener('message', onSocketMessage);
	}

	function openChat() {
		messages = [];
		cleanupSocket();
		createSocket(chatId);
	}
</script>

<form on:submit={openChat} class="flex flex-col">
	<label class="flex flex-col">
		Chat room
		<input bind:value={chatId} class="border" />
	</label>
	<button type="submit">Submit</button>
</form>

<form on:submit={submit} class="flex flex-col">
	<label class="flex flex-col">
		Input
		<input bind:value={input} class="border" />
	</label>
	<button type="submit">Submit</button>
</form>

<p>Messages</p>
<ul class="flex flex-col gap-2">
	{#each messages as m (m)}
		<li>{m}</li>
	{/each}
</ul>
