<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import { useMutation } from '@sveltestack/svelte-query';
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { twMerge } from 'tailwind-merge';
	import { deleteDocumentRequest } from './deleteDocumentRequest';
	import { Dialog } from '@/lib/components/dialog';

	let {
		documentId,
		documentName,
		class: className,
		...restprops
	}: HTMLButtonAttributes & { documentId: string; documentName: string } = $props();

	let isOpen = $state(false);
	let error = $state('');
	const mutation = useMutation(() => deleteDocumentRequest(documentId), {
		onSuccess: (res) => {
			if (res.error) {
				error = res.error.message;
				console.error(res.error);
			} else {
				location.href = '/';
			}
		}
	});
</script>

<Button
	{...restprops}
	class={twMerge('bg-destructive text-destructive-foreground', className)}
	onclick={() => (isOpen = true)}
/>

<Dialog open={isOpen}>
	<div class="flex flex-col gap-4 rounded bg-background p-4">
		<section class="text-center">
			Are you sure you want to delete:
			<p class="text-lg font-bold">{documentName}</p>
		</section>
		<section class="flex flex-row items-center justify-center gap-2">
			<Button disabled={$mutation.isLoading} onclick={() => (isOpen = false)}>Cancel</Button>
			<Button
				isLoading={$mutation.isLoading}
				disabled={$mutation.isLoading}
				onclick={() => $mutation.mutate()}
				class="bg-destructive text-destructive-foreground">Delete</Button
			>
		</section>
	</div>
</Dialog>
