import { ClientRequest, type ApiResponse } from '@/usecases/common/client';
import { deleteDocumentResponseSchema, type DeleteDocumentResponseSchema } from '../common';

export async function deleteDocumentRequest(
	id: string
): Promise<ApiResponse<DeleteDocumentResponseSchema>> {
	return await ClientRequest.send(`/api/documents/${id}`, deleteDocumentResponseSchema, {
		method: 'DELETE'
	});
}
