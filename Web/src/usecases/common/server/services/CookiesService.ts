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
			...this.authTokenConfig()
		});
	};

	public removeAccessToken = (cookies: Cookies) => {
		cookies.delete(this.accessTokenCookie, {
			...this.authTokenConfig()
		});
	};

	public removeAuthTokens = (cookies: Cookies) => {
		console.debug('Removing auth tokens...');
		this.removeAccessToken(cookies);
		this.removeRefreshToken(cookies);
		console.debug('Access token:', this.getAccessToken(cookies));
		console.debug('Refresh token:', this.getRefreshToken(cookies));
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
