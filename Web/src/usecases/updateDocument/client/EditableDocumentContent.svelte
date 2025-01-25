<script lang="ts">
	import type { HTMLTextareaAttributes } from 'svelte/elements';
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
</script>

<div class="relative flex w-full flex-col items-center">
	<section
		class="sticky top-0 flex w-full flex-col justify-between gap-4 gap-y-2 bg-background py-4 lg:top-14 lg:flex-row lg:p-4"
	>
		<EditableDocumentName id={docId} defaultValue={docName} />
		<a href={`/docs/${docId}`} class="hidden lg:block">
			<Button class="w-full">Render</Button>
		</a>
	</section>
	<BasicEditor
		{...restprops}
		documentId={docId}
		documentContent={docContent}
		class="min-h-screen w-full max-w-5xl resize-none border-none bg-input p-4 font-mono outline-none lg:p-28"
	></BasicEditor>
</div>
