<script lang="ts">
	import {
		Form,
		FormField,
		FormFieldError,
		FormFieldLabel,
		HiddenTextInput,
		TextInput
	} from '@/lib/components/form';
	import { Button } from '@/lib/components/interact';
	import { FormErrorState } from '@/lib/utils/FormErrorState';
	import { FormUtils } from '@/lib/utils/FormUtils';
	import { registerUserFormSchema, type RegisterUserResponseSchema } from '../common';
	import { z } from 'zod';
	import { useMutation } from '@sveltestack/svelte-query';
	import { registerUserRequest, type RegisterUserResult } from './registerUserRequest';

	const { errors, ...formErrors } = new FormErrorState(
		registerUserFormSchema.merge(
			z.object({
				passwordConfirm: z.string()
			})
		)
	);

	let { onsuccess }: { onsuccess?: (data: RegisterUserResult) => void } = $props();

	let error = $state('');

	function formSubmit(e: SubmitEvent) {
		formErrors.reset();
		error = '';
		const data = FormUtils.formData(e);
		formErrors.validate(data);

		if (data['password'] !== data['passwordConfirm'])
			formErrors.addError('passwordConfirm', 'Passwords do not match.');

		if (!formErrors.isValid()) return;
		$mutation.mutate(formErrors.getValidData());
	}

	const mutation = useMutation(registerUserRequest, {
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
			<FormFieldError>
				{e}
			</FormFieldError>
		{/each}
	</FormField>
	<FormField>
		<FormFieldLabel>
			Password
			<HiddenTextInput name="password" required />
		</FormFieldLabel>
		{#each $errors.password ?? [] as e}
			<FormFieldError>
				{e}
			</FormFieldError>
		{/each}
	</FormField>
	<FormField>
		<FormFieldLabel>
			Confirm password
			<HiddenTextInput name="passwordConfirm" required />
		</FormFieldLabel>
		{#each $errors.passwordConfirm ?? [] as e}
			<FormFieldError>
				{e}
			</FormFieldError>
		{/each}
	</FormField>

	<FormFieldError>
		{error}
	</FormFieldError>

	<Button type="submit" class="w-full" disabled={$mutation.isLoading}>Create Account</Button>
</Form>
