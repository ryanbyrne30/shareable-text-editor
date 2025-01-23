import { backendServer } from '@/usecases/common/server';
import { z } from 'zod';
import type { RefreshUserTokensResponseSchema } from '../common';

const expectedResponseSchema = z.object({
	access_token: z.string(),
	refresh_token: z.string()
});

export async function refreshUserTokens(
	refreshToken: string
): Promise<RefreshUserTokensResponseSchema> {
	const response = await backendServer.sendRequest('/api/v1/auth/refresh', expectedResponseSchema, {
		method: 'POST',
		body: { refresh_token: refreshToken }
	});

	return {
		accessToken: response.access_token,
		refreshToken: response.refresh_token
	};
}
