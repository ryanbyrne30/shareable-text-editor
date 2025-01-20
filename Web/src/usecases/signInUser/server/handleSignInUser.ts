import { z } from 'zod';
import { backendServer, Endpoint } from '@/usecases/common/server';
import { signInUserFormSchema, type SignInUserResponseSchema } from '../common';
import type { RequestEvent } from '@sveltejs/kit';
import { env } from '@/usecases/common/server/env';

const SECONDS_IN_MINUTE = 60;
const MINUTES_IN_HOUR = 60;
const MINUTES_IN_DAY = MINUTES_IN_HOUR * 24;

const expectedResponseSchema = z.object({
	access_token: z.string(),
	refresh_token: z.string()
});

export async function handleSignInUser({ request, cookies }: RequestEvent): Promise<Response> {
	const data = await Endpoint.parseRequest(request, signInUserFormSchema);
	const serverResponse = await backendServer.sendRequest(
		'/api/v1/auth/sign-in',
		expectedResponseSchema,
		{
			method: 'POST',
			body: data
		}
	);

	const cookieOptions: Parameters<typeof cookies.set>[2] = {
		path: '/',
		httpOnly: true,
		secure: env.NODE_ENV === 'production',
		sameSite: 'lax',
		maxAge: SECONDS_IN_MINUTE
	};
	cookies.set(env.ACCESS_TOKEN_COOKIE, serverResponse.access_token, {
		...cookieOptions,
		maxAge: env.JWT_ACCESS_TOKEN_EXPIRATION_MINUTES * SECONDS_IN_MINUTE
	});
	cookies.set(env.REFRESH_TOKEN_COOKIE, serverResponse.refresh_token, {
		...cookieOptions,
		maxAge: env.JWT_REFRESH_TOKEN_EXPIRATION_DAYS * SECONDS_IN_MINUTE * MINUTES_IN_DAY
	});

	const response: SignInUserResponseSchema = {
		message: 'Ok'
	};
	return Endpoint.jsonResponse(response);
}
