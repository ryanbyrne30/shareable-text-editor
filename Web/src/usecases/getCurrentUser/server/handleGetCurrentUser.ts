import type { RequestEvent } from '@sveltejs/kit';
import type { GetCurrentUserResponseSchema } from '../common';
import { backendServer, Endpoint } from '@/usecases/common/server';
import { z } from 'zod';
import { env } from '@/usecases/common/server/env';

const expectedResponseSchema = z.object({
	id: z.string(),
	username: z.string()
});

export async function handleGetCurrentUser({ cookies }: RequestEvent): Promise<Response> {
	const accessToken = cookies.get(env.ACCESS_TOKEN_COOKIE);
	if (!accessToken) return Endpoint.jsonResponse(null);

	const serverResponse = await backendServer.sendRequest(
		'/api/v1/users/me',
		expectedResponseSchema,
		{
			headers: {
				Authorization: `Bearer ${accessToken}`
			}
		}
	);
	const response: GetCurrentUserResponseSchema = {
		id: serverResponse.id,
		username: serverResponse.username
	};
	return Endpoint.jsonResponse(response);
}
