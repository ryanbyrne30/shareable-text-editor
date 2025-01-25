import { z } from 'zod';
import { env as clientEnv } from '$env/dynamic/public';

const envSchema = z.object({
	PUBLIC_DOCUMENT_WEBSOCKET_URL: z.string()
});

function buildEnv(): z.infer<typeof envSchema> {
	return envSchema.parse(clientEnv);
}

export const env = buildEnv();
