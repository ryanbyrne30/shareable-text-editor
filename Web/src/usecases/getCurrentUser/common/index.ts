import { z } from 'zod';

export const getCurrentUserResponseSchema = z
	.object({
		id: z.string(),
		username: z.string()
	})
	.nullable();
export type GetCurrentUserResponseSchema = z.infer<typeof getCurrentUserResponseSchema>;
