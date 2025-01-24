<script lang="ts">
	import EditableDocumentContent from '@/usecases/updateDocument/client/EditableDocumentContent.svelte';
	let { data } = $props();
</script>

<a href="/">Home</a>

<main class="flex min-h-[calc(100dvh-132px)] w-dvw flex-col items-center justify-start p-4">
	<EditableDocumentContent
		docId={data.document.id}
		docName={data.document.name}
		docContent={data.document.content}
	/>
</main>
