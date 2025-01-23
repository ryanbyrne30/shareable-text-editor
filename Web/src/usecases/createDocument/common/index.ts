import { z } from 'zod';

export const createDocumentResponseSchema = z.object({
	id: z.string()
});
export type CreateDocumentResponseSchema = z.infer<typeof createDocumentResponseSchema>;
