import { z } from 'zod';

export const getDocumentByIdResponseSchema = z.object({
	id: z.string(),
	name: z.string(),
	content: z.string()
});

export type GetDocumentByIdResponseSchema = z.infer<typeof getDocumentByIdResponseSchema>;
