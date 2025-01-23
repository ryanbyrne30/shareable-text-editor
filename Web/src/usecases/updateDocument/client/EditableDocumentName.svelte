<script lang="ts">
	import { TextInput } from '@/lib/components/form';
	import { useMutation } from '@sveltestack/svelte-query';
	import { onMount } from 'svelte';
	import type { HTMLInputAttributes } from 'svelte/elements';
	import { updateDocumentRequest } from './updateDocumentRequest';
	import type { UpdateDocumentRequestSchema } from '../common';

	let { id, defaultValue, ...restprops }: HTMLInputAttributes & { id: string } = $props();
	let value = $state(defaultValue ?? '');
	let span: HTMLSpanElement | null = null;
	let input: HTMLInputElement | null = $state(null);

	const mutation = useMutation(
		(updates: UpdateDocumentRequestSchema) => updateDocumentRequest(id, updates),
		{
			onError: (err) => {
				console.error(err);
				alert('Could not update document name');
			}
		}
	);

	function getSpanWidth() {
		if (!span) return 0;
		return span.getBoundingClientRect().width + 5;
	}

	function onchange() {
		if (!input) return;
		const name = input.value.trim();
		$mutation.mutate({ name });
	}

	function setInputWidth() {
		if (!span || !input) return;
		const width = getSpanWidth();
		input.style.width = `${width}px`;
	}

	onMount(setInputWidth);
</script>

<div class="relative text-3xl font-medium">
	<TextInput
		bind:me={input}
		bind:value
		oninput={(e) => {
			if (!span || !input) return;
			span.innerText = e.currentTarget.value;
			setInputWidth();
		}}
		{onchange}
		onfocusout={() => {
			if (!input) return;
			input.scrollLeft = 0;
		}}
		{...restprops}
		class="max-w-96 border-2 border-transparent bg-transparent p-0 outline-none hover:border-muted-foreground focus:border-border focus:bg-primary focus:text-primary-foreground"
		name="name"
		maxlength={100}
	/>
	<span bind:this={span} class="absolute whitespace-pre text-nowrap opacity-0">{value}</span>
</div>
