<script lang="ts">
	import { browser } from '$app/environment';
	import { Pagination } from '@/lib/components/pagination';
	import { userStore } from '@/usecases/common/client/stores';
	import { CreateDocumentButton } from '@/usecases/createDocument/client';
	import { getCurrentUserDocumentsRequest } from '@/usecases/getCurrentUserDocuments/client';
	import { useQuery } from '@sveltestack/svelte-query';
	import { twMerge } from 'tailwind-merge';

	let page = 1;
	let pageSize = 2;
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

<main class="m-auto flex w-full max-w-3xl flex-col gap-4 p-2 pt-16">
	<section class="flex w-full flex-row items-center justify-end">
		<CreateDocumentButton>New</CreateDocumentButton>
	</section>
	<section>
		<div
			class={twMerge(
				'h-48 w-full animate-pulse rounded-lg bg-border',
				browser && !$query.isLoading ? 'hidden' : ''
			)}
		></div>
		{#if !$query.isLoading}
			{#if $query.data?.data?.documents.length}
				<ul>
					{#each $query.data?.data?.documents ?? [] as d}
						<li>
							<a href={`/docs/${d.id}`}>
								<p class="border-b p-4">
									{d.name}
								</p>
							</a>
						</li>
					{/each}
				</ul>
			{:else}
				<p>No documents created</p>
			{/if}
		{/if}
	</section>

	<Pagination
		total={$query.data?.data?.total ?? 0}
		{pageSize}
		currentPage={page}
		onPageChange={(newPage) => {
			page = newPage;
			$query.refetch();
		}}
	/>
</main>
