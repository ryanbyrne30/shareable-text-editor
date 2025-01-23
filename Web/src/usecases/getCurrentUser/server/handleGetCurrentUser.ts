import type { RequestEvent } from '@sveltejs/kit';
import type { GetCurrentUserResponseSchema } from '../common';
import { backendServer, Endpoint } from '@/usecases/common/server';
import { z } from 'zod';

const expectedResponseSchema = z.object({
	id: z.string(),
	username: z.string()
});

export async function handleGetCurrentUser({ cookies }: RequestEvent): Promise<Response> {
	const serverResponse = await backendServer.sendAuthorizedRequest(
		cookies,
		'/api/v1/users/me',
		expectedResponseSchema
	);
	const response: GetCurrentUserResponseSchema = {
		id: serverResponse.id,
		username: serverResponse.username
	};
	return Endpoint.jsonResponse(response);
}
