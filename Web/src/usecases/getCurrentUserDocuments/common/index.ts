import { z } from 'zod';

export const getCurrentUserDocumentsRequestSchema = z.object({
	page: z
		.string()
		.transform((s) => parseInt(s))
		.optional(),
	pageSize: z
		.string()
		.transform((s) => parseInt(s))
		.optional(),
	sortBy: z.enum(['name', 'updated_at', 'created_at']).optional(),
	sortDirection: z.enum(['asc', 'desc']).optional()
});
export type GetCurrentUserDocumentsRequestSchema = z.infer<
	typeof getCurrentUserDocumentsRequestSchema
>;

export const getCurrentUserDocumentsResponseSchema = z.object({
	documents: z
		.object({
			id: z.string(),
			name: z.string(),
			createdAt: z.string().transform((s) => new Date(s)),
			updatedAt: z
				.string()
				.nullable()
				.transform((s) => (s ? new Date(s) : null))
		})
		.array(),
	total: z.number()
});
export type GetCurrentUserDocumentsResponseSchema = z.infer<
	typeof getCurrentUserDocumentsResponseSchema
>;
