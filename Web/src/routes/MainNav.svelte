<script>
	import { MenuIcon } from '@/lib/components/icon';
	import { Button } from '@/lib/components/interact';
	import { userStore } from '@/usecases/common/client/stores';
	import { SignOutUserButton } from '@/usecases/signOutUser/client';
	import { onMount } from 'svelte';
	import { twMerge } from 'tailwind-merge';

	let isOpen = $state(false);

	onMount(() => {
		const listItems = document.querySelectorAll('#main-nav li');
		listItems.forEach((el) => {
			el.addEventListener('click', () => {
				isOpen = false;
			});
		});
	});
</script>

<nav
	id="main-nav"
	class="pointer-events-none fixed left-0 right-0 top-0 z-30 flex flex-row justify-end p-2 lg:bg-background"
>
	<button
		type="button"
		class="pointer-events-auto rounded-full bg-background p-1 lg:hidden"
		onclick={() => (isOpen = !isOpen)}
	>
		<MenuIcon class="h-8 w-8" />
	</button>
	<aside
		class={twMerge(
			'pointer-events-auto fixed -z-10 flex h-dvh w-3/4 flex-col items-center justify-center gap-8 bg-background shadow-md transition-all lg:flex-row',
			isOpen ? 'left-1/4' : 'left-full',
			'lg:relative lg:left-0 lg:right-0 lg:top-0 lg:h-fit lg:w-full lg:justify-between'
		)}
	>
		<span class="hidden lg:block"></span>
		<ul class="flex flex-col gap-2 font-medium lg:flex-row lg:font-bold">
			<li>
				<a href="/">Documents</a>
			</li>
		</ul>

		<ul class="flex flex-col items-center gap-2 lg:flex-row">
			{#if $userStore === null}
				<li>
					<a href="/auth/sign-in">
						<Button>Sign In</Button>
					</a>
				</li>
				<li>
					<a href="/auth/register">
						<Button>Sign Up</Button>
					</a>
				</li>
			{:else}
				<li>
					<SignOutUserButton>Sign Out</SignOutUserButton>
				</li>
			{/if}
		</ul>
	</aside>
</nav>
<div
	class={twMerge(
		'fixed z-10 h-dvh w-dvw backdrop-blur-sm backdrop-brightness-75',
		isOpen ? '' : 'hidden'
	)}
></div>
