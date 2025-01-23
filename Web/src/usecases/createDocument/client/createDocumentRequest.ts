import { ClientRequest, type ApiResponse } from '@/usecases/common/client/ClientRequest';
import { createDocumentResponseSchema, type CreateDocumentResponseSchema } from '../common';

export async function createDocumentRequest(): Promise<ApiResponse<CreateDocumentResponseSchema>> {
	return await ClientRequest.send('/api/documents', createDocumentResponseSchema, {
		method: 'POST'
	});
}
