<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import { ClientSanitization } from '@/usecases/common/client';
	import DeleteDocumentButton from '@/usecases/deleteDocument/client/DeleteDocumentButton.svelte';
	import { marked } from 'marked';
	import { onMount } from 'svelte';

	let { data } = $props();
	let docEl: HTMLElement | null = null;

	onMount(async () => {
		if (!docEl) return;
		const sanitizedHtml = ClientSanitization.sanitizeHtml(data.document.content);
		docEl.innerHTML = await marked.parse(sanitizedHtml);
	});
</script>

<section class="flex flex-row flex-wrap justify-between gap-x-4 gap-y-2 border-b p-4 py-3 lg:pt-16">
	<h1 class="p-0">{data.document.name}</h1>
	<div class="flex flex-row gap-2">
		<a class="hidden lg:block" href={`/docs/${data.document.id}/edit`}>
			<Button class="w-full">Edit</Button>
		</a>
		<DeleteDocumentButton
			class="hidden lg:block"
			documentId={data.document.id}
			documentName={data.document.name}>Delete</DeleteDocumentButton
		>
	</div>
</section>
<article bind:this={docEl} class="p-4"></article>
<section class="absolute bottom-0 left-0 right-0 flex flex-row gap-2 p-2 lg:hidden">
	<a href={`/docs/${data.document.id}/edit`} class="w-full">
		<Button class="w-full">Edit</Button>
	</a>
	<DeleteDocumentButton documentId={data.document.id} documentName={data.document.name}
		>Delete</DeleteDocumentButton
	>
</section>
