import { backendServer, Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { z } from 'zod';
import type { CreateDocumentResponseSchema } from '../common';

const expectedResponseSchema = z.object({
	id: z.string()
});

export async function handleCreateDocument({ cookies }: RequestEvent) {
	const serverResponse = await backendServer.sendAuthorizedRequest(
		cookies,
		'/api/v1/documents',
		expectedResponseSchema,
		{
			method: 'POST'
		}
	);
	const response: CreateDocumentResponseSchema = {
		id: serverResponse.id
	};
	return Endpoint.jsonResponse(response);
}
