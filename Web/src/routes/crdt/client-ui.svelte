<script lang="ts">
	import { twMerge } from 'tailwind-merge';
	import { ActionBuilder, ClientEventDispatcher, type ClientState } from './client';

	export let index: number;
	export let state: ClientState;

	let isFocused = false;
	let input: HTMLTextAreaElement | null = null;
	const actionBuilder = new ActionBuilder();

	function onKeyDown(key: string) {
		actionBuilder.addKey(key);
	}

	function onsubmit() {
		console.log('Send');
	}

	ClientEventDispatcher.addKeyDownHandler(onKeyDown);
</script>

<form class="flex flex-col gap-2" {onsubmit}>
	<label class="flex flex-col">
		Client {index}
		<textarea
			bind:this={input}
			class={twMerge('border-2 p-2', state.isFocused ? 'bg-red-100' : '')}
			rows="5"
			onfocusin={() => {
				isFocused = false;
				ClientEventDispatcher.changeClientState(index, { isFocused: true });
			}}
			onfocusout={() => {
				isFocused = true;
				ClientEventDispatcher.changeClientState(index, { isFocused: false });
			}}
			onselectionchange={() => {
				if (input === null) return;
				actionBuilder.setCursor(input.selectionStart);
			}}
		></textarea>
	</label>
	<button type="submit" class="rounded bg-blue-200 px-4 py-2">Send</button>
</form>
