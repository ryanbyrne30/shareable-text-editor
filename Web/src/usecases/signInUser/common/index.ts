import { z } from 'zod';

export const signInUserFormSchema = z.object({
	username: z.string(),
	password: z.string()
});
export type SignInUserFormSchema = z.infer<typeof signInUserFormSchema>;

export const signInUserResponseSchema = z.object({
	message: z.string()
});
export type SignInUserResponseSchema = z.infer<typeof signInUserResponseSchema>;
