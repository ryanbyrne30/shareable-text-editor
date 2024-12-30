<script lang="ts">
	import { onMount } from 'svelte';

	let { data } = $props();

	let socket: WebSocket | null = null;

	function setup() {
		socket = new WebSocket(`http://localhost:5001/ws/docs/doc/${data.id}`);
		socket.addEventListener('open', () => console.debug('Socket connected'));
		socket.addEventListener('close', () => console.debug('Socket disconnected'));
		socket.addEventListener('error', (e) => console.error('Socket error:', e));
		socket.addEventListener('message', (e) => console.debug('Received:', e.data));
	}

	function onInput(input: string) {
		if (socket === null) return;
		socket.send(input);
	}

	onMount(setup);
</script>

<textarea oninput={(e) => onInput(e.currentTarget.value)} class="border"></textarea>
