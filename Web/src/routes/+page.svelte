<script lang="ts">
	import { browser } from '$app/environment';
	import { userStore } from '@/usecases/common/client/stores';
	import { CreateDocumentButton } from '@/usecases/createDocument/client';
	import { getCurrentUserDocumentsRequest } from '@/usecases/getCurrentUserDocuments/client';
	import { useQuery } from '@sveltestack/svelte-query';
	import { twMerge } from 'tailwind-merge';

	let page = 1;
	let pageSize = 10;
	let sortBy: 'updated_at' | 'created_at' | 'name' = 'updated_at';
	let sortDirection: 'asc' | 'desc' = 'desc';

	const query = useQuery(
		`getCurrentUserDocumentsRequest ${page} ${pageSize} ${sortBy} ${sortDirection}`,
		() =>
			getCurrentUserDocumentsRequest({
				page,
				pageSize,
				sortBy,
				sortDirection
			}),
		{
			enabled: browser
		}
	);
</script>

<h1>Hello {$userStore?.username ?? 'User'}!</h1>

<section class="flex w-dvw flex-col items-center justify-center p-16">
	<CreateDocumentButton>Create document</CreateDocumentButton>
</section>

<section>
	<div
		class={twMerge(
			'h-48 w-full animate-pulse rounded-lg bg-border',
			browser && !$query.isLoading ? 'hidden' : ''
		)}
	></div>
	{#if !$query.isLoading}
		<ul>
			{#each $query.data?.data?.documents ?? [] as d}
				<li>
					<a href={`/docs/${d.id}/edit`}>
						<p class="border-b p-4">
							{d.name}
						</p>
					</a>
				</li>
			{/each}
		</ul>
	{/if}
</section>
