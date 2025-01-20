import { registerUserFormSchema, type RegisterUserResponseSchema } from '../common';
import { z } from 'zod';
import type { RequestEvent } from '@sveltejs/kit';
import { Endpoint } from '@/server/Endpoint';
import { backendServer } from '@/server/services';

const expectedResponseSchema = z.object({
	id: z.string()
});

export async function handleRegisterUser({ request }: RequestEvent): Promise<Response> {
	const data = await Endpoint.parseRequest(request, registerUserFormSchema);
	const serverResponse = await backendServer.sendRequest('/api/v1/users', expectedResponseSchema, {
		method: 'POST',
		body: data
	});
	const response: RegisterUserResponseSchema = {
		id: serverResponse.id
	};
	return Endpoint.jsonResponse(response);
}
