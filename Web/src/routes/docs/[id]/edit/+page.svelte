<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import EditableDocumentContent from '@/usecases/updateDocument/client/EditableDocumentContent.svelte';
	let { data } = $props();
</script>

<main class="flex min-h-[calc(100dvh-132px)] w-dvw flex-col items-center justify-start p-4 pt-0">
	<EditableDocumentContent
		docId={data.document.id}
		docName={data.document.name}
		docContent={data.document.content}
	/>
</main>
<section
	class="fixed bottom-0 left-0 right-0 p-2 backdrop-blur-sm backdrop-brightness-75 lg:hidden"
>
	<a href={`/docs/${data.document.id}`}>
		<Button class="w-full">Render</Button>
	</a>
</section>
