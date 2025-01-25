<script lang="ts">
	import { ChevronIcon } from '../icon';
	import { Button } from '../interact';

	let {
		total,
		pageSize,
		currentPage,
		onPageChange
	}: {
		total: number;
		pageSize: number;
		currentPage?: number;
		onPageChange?: (page: number) => void;
	} = $props();
	let page = $state(currentPage ?? 1);
	let hasMore = $derived(page * pageSize < total);
	let hasLess = $derived(page > 1);

	function changePage(newPage: number) {
		page = newPage;
		if (onPageChange) onPageChange(newPage);
	}
</script>

<div class="flex w-full flex-row items-center justify-between">
	{#if hasLess}
		<Button class="flex flex-row flex-nowrap pl-2" onclick={() => changePage(page - 1)}
			><ChevronIcon /> Prev</Button
		>
	{:else}
		<span></span>
	{/if}
	{#if hasMore}
		<Button class="flex flex-row flex-nowrap pr-2" onclick={() => changePage(page + 1)}
			>Next <ChevronIcon class="rotate-180" /></Button
		>
	{:else}
		<span></span>
	{/if}
</div>
