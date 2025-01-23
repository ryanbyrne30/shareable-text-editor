import { env } from '../env';
import { BackendServer } from './BackendServer';
import { CookiesService } from './CookiesService';

export const cookiesService = new CookiesService(
	env.ACCESS_TOKEN_COOKIE,
	env.REFRESH_TOKEN_COOKIE,
	env.JWT_ACCESS_TOKEN_EXPIRATION_MINUTES,
	env.JWT_REFRESH_TOKEN_EXPIRATION_DAYS,
	env.NODE_ENV
);

export const backendServer = new BackendServer(env.BACKEND_SERVER_URL, cookiesService);
