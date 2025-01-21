import { cookiesService, Endpoint } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import type { SignOutUserResponseSchema } from '../common';

export async function handleSignOutUser({ cookies }: RequestEvent): Promise<Response> {
	cookiesService.removeAuthTokens(cookies);
	const response: SignOutUserResponseSchema = { message: 'ok' };
	return Endpoint.jsonResponse(response);
}
