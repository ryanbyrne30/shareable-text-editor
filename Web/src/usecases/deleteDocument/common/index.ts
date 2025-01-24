import { z } from 'zod';

export const deleteDocumentResponseSchema = z.object({ message: z.string() });
export type DeleteDocumentResponseSchema = z.infer<typeof deleteDocumentResponseSchema>;
