import { z } from 'zod';

export const updateDocumentRequestSchema = z.object({
	name: z.string().optional(),
	content: z.string().optional()
});
export type UpdateDocumentRequestSchema = z.infer<typeof updateDocumentRequestSchema>;

export const updateDocumentResponseSchema = z.object({ message: z.string() });
export type UpdateDocumentResponseSchema = z.infer<typeof updateDocumentResponseSchema>;
