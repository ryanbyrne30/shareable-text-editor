import type { Cookies } from '@sveltejs/kit';

export type CookieOptions = Parameters<Cookies['set']>[2];

export class CookiesService {
	constructor(
		private accessTokenCookie: string,
		private refreshTokenCookie: string,
		private accessTokenExpirationMinutes: number,
		private refreshTokenExpirationDays: number,
		private nodeEnv: 'production' | 'development'
	) {}

	public getAccessToken = (cookies: Cookies) => {
		return cookies.get(this.accessTokenCookie);
	};

	public getRefreshToken = (cookies: Cookies) => {
		return cookies.get(this.refreshTokenCookie);
	};

	public setAccessToken = (cookies: Cookies, token: string) => {
		cookies.set(this.accessTokenCookie, token, {
			...this.authTokenConfig(),
			maxAge: this.accessTokenExpirationMinutes * 60
		});
	};

	public setRefreshToken = (cookies: Cookies, token: string) => {
		cookies.set(this.refreshTokenCookie, token, {
			...this.authTokenConfig(),
			maxAge: this.refreshTokenExpirationDays * 60 * 60 * 24
		});
	};

	public removeRefreshToken = (cookies: Cookies) => {
		cookies.delete(this.refreshTokenCookie, {
			path: this.authTokenConfig().path
		});
	};

	public removeAccessToken = (cookies: Cookies) => {
		cookies.delete(this.accessTokenCookie, {
			path: this.authTokenConfig().path
		});
	};

	public removeAuthTokens = (cookies: Cookies) => {
		this.removeAccessToken(cookies);
		this.removeRefreshToken(cookies);
	};

	private authTokenConfig = (): CookieOptions => {
		return {
			path: '/',
			httpOnly: true,
			secure: this.nodeEnv === 'production',
			sameSite: 'lax'
		};
	};
}
