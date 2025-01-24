import { BadRequestError, Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { serverDeleteDocumentRequest } from './serverDeleteDocumentRequest';

export async function handleDeleteDocument({ cookies, params }: RequestEvent): Promise<Response> {
	const id = params['id'];
	if (!id) throw new BadRequestError({ message: 'Id not given' });
	const response = await serverDeleteDocumentRequest(cookies, id);
	return Endpoint.jsonResponse(response);
}
