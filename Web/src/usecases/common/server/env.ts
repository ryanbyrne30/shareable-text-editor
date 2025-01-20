import { z } from 'zod';
import { env as serverEnv } from '$env/dynamic/private';

const envSchema = z.object({
	NODE_ENV: z.enum(['production', 'development']).default('production'),
	BACKEND_SERVER_URL: z.string().url(),
	ACCESS_TOKEN_COOKIE: z.string().default('access_token'),
	REFRESH_TOKEN_COOKIE: z.string().default('refresh_token'),
	JWT_ISSUER: z.string(),
	JWT_AUDIENCE: z.string(),
	JWT_KEY: z.string(),
	JWT_ACCESS_TOKEN_EXPIRATION_MINUTES: z
		.string()
		.default('30')
		.transform((s) => parseInt(s)),
	JWT_REFRESH_TOKEN_EXPIRATION_DAYS: z
		.string()
		.default('7')
		.transform((s) => parseInt(s))
});

function buildEnv(): z.infer<typeof envSchema> {
	return envSchema.parse(serverEnv);
}

export const env = buildEnv();
