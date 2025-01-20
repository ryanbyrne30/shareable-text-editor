import { z } from 'zod';
import { env as serverEnv } from '$env/dynamic/private';

const envSchema = z.object({
	BACKEND_SERVER_URL: z.string().url()
});

function buildEnv(): z.infer<typeof envSchema> {
	return envSchema.parse(serverEnv);
}

export const env = buildEnv();
