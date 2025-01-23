import { ClientRequest } from '@/usecases/common/client';
import { updateDocumentResponseSchema, type UpdateDocumentRequestSchema } from '../common';

export async function updateDocumentRequest(id: string, data: UpdateDocumentRequestSchema) {
	await ClientRequest.send(`/api/documents/${id}`, updateDocumentResponseSchema, {
		method: 'PATCH',
		body: data
	});
}
