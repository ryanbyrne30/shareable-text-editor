import { Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { getCurrentUserDocumentsRequestSchema } from '../common';
import { serverGetCurrentUserDocumentsRequest } from './serverGetCurrentUserDocumentsRequest';

export async function handleGetCurrentUserDocuments({
	cookies,
	url
}: RequestEvent): Promise<Response> {
	const query = Endpoint.parseQuery(url, getCurrentUserDocumentsRequestSchema);
	const response = await serverGetCurrentUserDocumentsRequest(cookies, query);
	return Endpoint.jsonResponse(response);
}
