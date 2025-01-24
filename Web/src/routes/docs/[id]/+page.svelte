<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import { ClientSanitization } from '@/usecases/common/client';
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

<section class="flex flex-row flex-wrap justify-between gap-x-4 gap-y-2 border-b p-4">
	<h1>{data.document.name}</h1>
	<a href={`/docs/${data.document.id}/edit`}>
		<Button>Edit</Button>
	</a>
</section>
<article bind:this={docEl} class="p-4"></article>
