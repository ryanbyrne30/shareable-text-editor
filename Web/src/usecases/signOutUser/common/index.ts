import { z } from 'zod';

export const signOutUserResponseSchema = z.object({ message: z.string() });
export type SignOutUserResponseSchema = z.infer<typeof signOutUserResponseSchema>;
