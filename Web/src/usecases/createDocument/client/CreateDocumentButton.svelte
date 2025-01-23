<script lang="ts">
	import { Button } from '@/lib/components/interact';
	import type { HTMLButtonAttributes } from 'svelte/elements';

	let { ...restProps }: HTMLButtonAttributes = $props();
</script>

<Button {...restProps} />
