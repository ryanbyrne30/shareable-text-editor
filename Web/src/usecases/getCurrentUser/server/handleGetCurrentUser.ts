import type { Cookies, RequestEvent } from '@sveltejs/kit';
import type { GetCurrentUserResponseSchema } from '../common';
import { backendServer, BadRequestError, cookiesService, Endpoint } from '@/usecases/common/server';
import { z } from 'zod';
import { env } from '@/usecases/common/server/env';
import { HttpStatusCode } from '@/usecases/common/common';
import { refreshUserTokens } from '@/usecases/refreshUserTokens/server';

const expectedResponseSchema = z.object({
	id: z.string(),
	username: z.string()
});

export async function handleGetCurrentUser(event: RequestEvent, attempts = 0): Promise<Response> {
	if (attempts > 1) {
		cookiesService.removeAuthTokens(event.cookies);
		throw new BadRequestError({ message: 'Unauthorized', status: HttpStatusCode.Unauthorized });
	}

	const accessToken = event.cookies.get(env.ACCESS_TOKEN_COOKIE);
	if (!accessToken) return await refreshTokensAndRetry(event);

	try {
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
	} catch (err) {
		if (
			err instanceof BadRequestError &&
			(err.status === HttpStatusCode.Unauthorized || err.status === HttpStatusCode.Forbidden)
		) {
			return await refreshTokensAndRetry(event);
		}
		throw err;
	}
}

async function refreshTokensAndRetry(event: RequestEvent) {
	const refreshToken = event.cookies.get(env.REFRESH_TOKEN_COOKIE);
	if (!refreshToken) return Endpoint.jsonResponse(null);

	const newTokens = await refreshUserTokens(refreshToken);
	setNewTokens(event.cookies, newTokens.accessToken, newTokens.refreshToken);
	return await handleGetCurrentUser(event, 1);
}

async function setNewTokens(cookies: Cookies, accessToken: string, refreshToken: string) {
	cookiesService.setAccessToken(cookies, accessToken);
	cookiesService.setRefreshToken(cookies, refreshToken);
}
