import { z } from 'zod';

export const registerUserFormSchema = z.object({
	username: z
		.string()
		.trim()
		.min(3, 'Must be at least 3 characters.')
		.max(20, 'Must not exceed 20 characters.')
		.regex(
			new RegExp(/^[a-zA-Z0-9_-]+$/),
			'Only alphanumeric characters, underscore and hyphens allowed.'
		),
	password: z
		.string()
		.min(8, 'Must be at least 3 characters.')
		.max(100, 'Must not exceed 100 characters')
		.regex(new RegExp(/[a-z]/), 'Must contain a lowercase letter.')
		.regex(new RegExp(/[A-Z]/), 'Must contain an uppercase letter.')
		.regex(new RegExp(/[0-9]/), 'Must contain a number')
		.regex(new RegExp(/[!@#$%^&*()_+-=`~{}[\]\\|;:'",<.>/?']/), 'Must contain a special character')
});
export type RegisterUserFormSchema = z.infer<typeof registerUserFormSchema>;

export const registerUserResponseSchema = z.object({
	id: z.string()
});
export type RegisterUserResponseSchema = z.infer<typeof registerUserResponseSchema>;
