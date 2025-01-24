import { backendServer } from '@/usecases/common/server';
import type {
	GetCurrentUserDocumentsRequestSchema,
	GetCurrentUserDocumentsResponseSchema
} from '../common';
import type { Cookies } from '@sveltejs/kit';
import { z } from 'zod';

const expectedResponseSchema = z.object({
	documents: z
		.object({
			id: z.string(),
			name: z.string(),
			created_at: z.string().transform((s) => new Date(s)),
			updated_at: z
				.string()
				.nullable()
				.transform((s) => (s ? new Date(s) : null))
		})
		.array(),
	total: z.number()
});

export async function serverGetCurrentUserDocumentsRequest(
	cookies: Cookies,
	request: GetCurrentUserDocumentsRequestSchema
): Promise<GetCurrentUserDocumentsResponseSchema> {
	const searchParams = new URLSearchParams({
		page: `${request.page ?? 1}`,
		page_size: `${request.pageSize ?? 10}`,
		sort_by: request.sortBy ?? 'updated_at',
		sort_direction: request.sortDirection ?? 'desc'
	});

	const serverResponse = await backendServer.sendAuthorizedRequest(
		cookies,
		'/api/v1/users/me/documents?' + searchParams.toString(),
		expectedResponseSchema
	);

	const response: GetCurrentUserDocumentsResponseSchema = {
		documents: serverResponse.documents.map((d) => ({
			id: d.id,
			name: d.name,
			createdAt: d.created_at,
			updatedAt: d.updated_at
		})),
		total: serverResponse.total
	};

	return response;
}
