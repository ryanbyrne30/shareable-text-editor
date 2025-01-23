<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import { useMutation } from '@sveltestack/svelte-query';
	import type { HTMLButtonAttributes } from 'svelte/elements';
	import { createDocumentRequest } from './createDocumentRequest';

	let { ...restProps }: HTMLButtonAttributes = $props();
	const mutation = useMutation(createDocumentRequest, {
		onSuccess: (res) => {
			if (res.error) {
				console.error('Error when creating document');
				console.error(res.error);
				throw new Error(res.error.message);
			}
			location.href = `/docs/${res.data?.id}/edit`;
		}
	});
</script>

<Button {...restProps} onclick={() => $mutation.mutate()} isLoading={$mutation.isLoading} />
