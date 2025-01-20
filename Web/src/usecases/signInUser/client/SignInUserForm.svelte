<script lang="ts">
	import {
		Form,
		FormField,
		FormFieldError,
		FormFieldLabel,
		HiddenTextInput,
		TextInput
	} from '@/lib/components/form';
	import { FormErrorState, FormUtils } from '@/usecases/common/client';
	import { signInUserFormSchema } from '../common';
	import { Button } from '@/lib/components/interact';
	import { useMutation } from '@sveltestack/svelte-query';
	import { signInUserRequest, type SignInUserResult } from './signInUserRequest';

	let { onsuccess }: { onsuccess?: (data: SignInUserResult) => void } = $props();
	const { errors, ...formState } = new FormErrorState(signInUserFormSchema);
	let error = $state('');

	function formSubmit(e: SubmitEvent) {
		formState.reset();
		error = '';
		const data = FormUtils.formData(e);
		formState.validate(data);
		if (!formState.isValid()) return;
		$mutation.mutate(formState.getValidData());
	}

	const mutation = useMutation(signInUserRequest, {
		onSuccess: (result) => {
			if (result.error) {
				error = result.error.message;
				errors.set({ ...$errors, ...result.error.errors });
			} else if (onsuccess) {
				onsuccess(result);
			}
		}
	});
</script>

<Form onsubmit={formSubmit} class="w-72">
	<FormField>
		<FormFieldLabel>
			Username
			<TextInput type="text" name="username" required />
		</FormFieldLabel>
		{#each $errors.username ?? [] as e}
			<FormFieldError>{e}</FormFieldError>
		{/each}
	</FormField>
	<FormField>
		<FormFieldLabel>
			Password
			<HiddenTextInput name="password" required />
		</FormFieldLabel>
		{#each $errors.password ?? [] as e}
			<FormFieldError>{e}</FormFieldError>
		{/each}
	</FormField>

	<FormFieldError>{error}</FormFieldError>

	<Button type="submit" class="w-full">Sign In</Button>
</Form>
