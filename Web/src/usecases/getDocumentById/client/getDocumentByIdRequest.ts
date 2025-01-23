import { ClientRequest } from '@/usecases/common/client';
import { getDocumentByIdResponseSchema, type GetDocumentByIdResponseSchema } from '../common';
import type { ApiResponse } from '@/usecases/common/client/ClientRequest';

export async function getDocumentByIdRequest(
	id: string
): Promise<ApiResponse<GetDocumentByIdResponseSchema>> {
	return await ClientRequest.send(`/api/documents/${id}`, getDocumentByIdResponseSchema);
}
