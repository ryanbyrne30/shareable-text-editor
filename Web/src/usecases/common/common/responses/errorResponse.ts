import { z } from 'zod';

export const errorResponseSchema = z.object({
	message: z.string(),
	errors: z.record(z.string(), z.array(z.string()).optional()).optional()
});

export type ErrorResponseSchema = z.infer<typeof errorResponseSchema>;
