import { z } from 'zod';

export const backendErrorResponseSchema = z.object({
	message: z.string(),
	errors: z.record(z.string(), z.array(z.string()).optional()).optional()
});

export type BackendErrorResponseData = z.infer<typeof backendErrorResponseSchema>;
