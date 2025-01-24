import { ClientRequest } from '@/usecases/common/client';
import {
	getCurrentUserDocumentsResponseSchema,
	type GetCurrentUserDocumentsRequestSchema,
	type GetCurrentUserDocumentsResponseSchema
} from '../common';
import type { ApiResponse } from '@/usecases/common/client/ClientRequest';

export async function getCurrentUserDocumentsRequest(
	query: GetCurrentUserDocumentsRequestSchema
): Promise<ApiResponse<GetCurrentUserDocumentsResponseSchema>> {
	const queryParams = new URLSearchParams({
		page: `${query.page ?? 1}`,
		pageSize: `${query.pageSize ?? 10}`,
		sortBy: query.sortBy ?? 'updated_at',
		sortDirection: query.sortDirection ?? 'desc'
	});
	return await ClientRequest.send(
		'/api/users/me/documents?' + queryParams.toString(),
		getCurrentUserDocumentsResponseSchema
	);
}
