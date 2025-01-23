import type { z, ZodType } from 'zod';
import { Endpoint } from '../Endpoint';
import type { CookiesService } from './CookiesService';
import type { Cookies } from '@sveltejs/kit';
import { UnauthenticatedRequestError, UnauthorizedRequestError } from '../errors';
import { refreshUserTokens } from '@/usecases/refreshUserTokens/server';

export class BackendServer {
	constructor(
		private serverUrl: string,
		private cookiesService: CookiesService
	) {}

	public sendRequest = async <T extends ZodType>(
		endpoint: string,
		responseSchema: T,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<z.infer<T>> => {
		return await Endpoint.sendJsonRequest(this.serverUrl + endpoint, responseSchema, options);
	};

	public sendAuthorizedRequest = async <T extends ZodType>(
		cookies: Cookies,
		endpoint: string,
		responseSchema: T,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<z.infer<T>> => {
		return await this._sendAuthorizedRequest(cookies, endpoint, responseSchema, 0, options);
	};

	private _sendAuthorizedRequest = async <T extends ZodType>(
		cookies: Cookies,
		endpoint: string,
		responseSchema: T,
		attempts: number = 0,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<z.infer<T>> => {
		if (attempts > 1) throw new UnauthorizedRequestError();
		try {
			const accessToken = this.cookiesService.getAccessToken(cookies);
			return await this.sendRequest(endpoint, responseSchema, {
				...options,
				headers: {
					...options?.headers,
					Authorization: `Bearer ${accessToken}`
				}
			});
		} catch (err) {
			if (err instanceof UnauthorizedRequestError || err instanceof UnauthenticatedRequestError) {
				await this._refreshToken(cookies);
				return await this._sendAuthorizedRequest(
					cookies,
					endpoint,
					responseSchema,
					attempts + 1,
					options
				);
			}
			throw err;
		}
	};

	private _refreshToken = async (cookies: Cookies): Promise<string> => {
		const refreshToken = this.cookiesService.getRefreshToken(cookies);
		if (!refreshToken) throw new UnauthenticatedRequestError();
		const newTokens = await refreshUserTokens(refreshToken);
		this.cookiesService.setAccessToken(cookies, newTokens.accessToken);
		this.cookiesService.setRefreshToken(cookies, newTokens.refreshToken);
		return newTokens.accessToken;
	};
}
