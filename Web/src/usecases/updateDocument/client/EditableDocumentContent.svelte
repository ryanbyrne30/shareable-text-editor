<script lang="ts">
	import { useMutation } from '@sveltestack/svelte-query';
	import type { HTMLTextareaAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	import { updateDocumentRequest } from './updateDocumentRequest';
	import { beforeNavigate } from '$app/navigation';
	import { onMount } from 'svelte';
	import { Button } from '@/lib/components/interact';
	import EditableDocumentName from './EditableDocumentName.svelte';
	import { BasicEditor } from '@/lib/editors/basicEditor';

	let {
		docId,
		docName,
		docContent,
		defaultValue,
		class: className,
		...restprops
	}: HTMLTextareaAttributes & { docId: string; docName: string; docContent: string } = $props();
	let content = $state(docContent ?? '');
	let debounceMs: number = 3000;
	let debounceTimer: NodeJS.Timeout | number = 0;
	let docState = $state<'saved' | 'unsaved' | 'saving'>('saved');
	let minSavingMs = 1000;
	let minSavingTimer: NodeJS.Timeout | number = 0;

	const mutation = useMutation(() => updateDocumentRequest(docId, { content }), {
		onSuccess: () => {
			if (minSavingTimer) return;
			docState = 'saved';
		}
	});

	async function oninput() {
		docState = 'unsaved';
		clearTimeout(debounceTimer);
		debounceTimer = setTimeout(savedocument, debounceMs);
	}

	function savedocument() {
		$mutation.mutate();
		clearTimeout(debounceTimer);
		docState = 'saving';
		minSavingTimer = setTimeout(() => {
			docState = 'saved';
			clearTimeout(minSavingTimer);
		}, minSavingMs);
	}

	beforeNavigate(savedocument);

	onMount(() => {
		window.addEventListener('beforeunload', (e) => {
			if (docState === 'saved') return;
			savedocument();
			confirm('Changes have not been saved yet');
			e.preventDefault();
			return '';
		});
	});
</script>

<div class="relative flex w-full flex-col items-center">
	<section
		class="sticky top-0 flex w-full flex-col justify-between gap-4 gap-y-2 bg-background py-4 lg:top-14 lg:flex-row lg:p-4"
	>
		<EditableDocumentName id={docId} defaultValue={docName} />
		<div class="flex flex-row gap-4">
			<Button
				onclick={() => {
					if (docState === 'unsaved') savedocument();
				}}
				class={twMerge(
					'border-none bg-transparent p-0 text-foreground opacity-70',
					docState === 'saving' ? 'animate-pulse' : docState === 'unsaved' ? 'opacity-100' : ''
				)}
				disabled={docState !== 'unsaved'}
				>{docState === 'saving'
					? 'Saving...'
					: docState === 'unsaved'
						? 'Unsaved'
						: 'Saved'}</Button
			>

			<a href={`/docs/${docId}`} class="hidden lg:block">
				<Button class="w-full">Render</Button>
			</a>
		</div>
	</section>
	<BasicEditor
		{...restprops}
		{oninput}
		class="min-h-screen w-full max-w-5xl resize-none border-none bg-input p-4 font-mono outline-none lg:p-28"
	></BasicEditor>
</div>
