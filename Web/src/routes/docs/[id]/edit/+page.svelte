<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import EditableDocumentContent from '@/usecases/updateDocument/client/EditableDocumentContent.svelte';
	let { data } = $props();
</script>

<nav class="flex flex-row gap-4 p-2 lg:p-4">
	<a href={'/'}>
		<Button>Home</Button>
	</a>
	<a href={`/docs/${data.document.id}`}>
		<Button>Render</Button>
	</a>
</nav>

<main class="flex min-h-[calc(100dvh-132px)] w-dvw flex-col items-center justify-start p-4">
	<EditableDocumentContent
		docId={data.document.id}
		docName={data.document.name}
		docContent={data.document.content}
	/>
</main>
