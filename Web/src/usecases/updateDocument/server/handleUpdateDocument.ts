import { backendServer, BadRequestError, Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { z } from 'zod';
import { updateDocumentRequestSchema, type UpdateDocumentResponseSchema } from '../common';

const expectedResponseSchema = z.object({ message: z.string() });

export async function handleUpdateDocument({
	cookies,
	params,
	request
}: RequestEvent): Promise<Response> {
	const id = params['id'];
	const body = await Endpoint.parseRequest(request, updateDocumentRequestSchema);
	if (!id) throw new BadRequestError({ message: 'Id not given' });
	await backendServer.sendAuthorizedRequest(
		cookies,
		`/api/v1/documents/${id}`,
		expectedResponseSchema,
		{
			method: 'PATCH',
			body
		}
	);
	const response: UpdateDocumentResponseSchema = { message: 'Ok' };
	return Endpoint.jsonResponse(response);
}
