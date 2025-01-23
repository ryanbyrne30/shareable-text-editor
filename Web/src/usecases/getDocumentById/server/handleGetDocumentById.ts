import { Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { serverGetDocumentByIdRequest } from './serverGetDocumentByIdRequest';

export async function handleGetDocumentById(event: RequestEvent): Promise<Response> {
	const response = await serverGetDocumentByIdRequest(event);
	return Endpoint.jsonResponse(response);
}
