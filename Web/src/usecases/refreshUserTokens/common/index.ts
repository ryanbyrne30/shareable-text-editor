import { z } from 'zod';

export const refreshUserTokensResponseSchema = z.object({
	accessToken: z.string(),
	refreshToken: z.string()
});
export type RefreshUserTokensResponseSchema = z.infer<typeof refreshUserTokensResponseSchema>;
