<script lang="ts">
	import { twMerge } from 'tailwind-merge';
	import { EyeHiddenIcon, EyeIcon } from '../icon';
	import TextInput from './TextInput.svelte';
	import type { HTMLInputAttributes } from 'svelte/elements';

	let { class: className, ...restProps }: HTMLInputAttributes = $props();

	let inputType: 'password' | 'text' = $state('password');

	function onclick() {
		if (inputType === 'password') inputType = 'text';
		else inputType = 'password';
	}
</script>

<div class="relative flex flex-row items-center">
	<TextInput {...restProps} class={twMerge('pr-10', className)} type={inputType} />
	<button type="button" {onclick} class="absolute right-0 bg-primary-foreground p-1">
		{#if inputType === 'password'}
			<EyeIcon />
		{:else}
			<EyeHiddenIcon />
		{/if}
	</button>
</div>
