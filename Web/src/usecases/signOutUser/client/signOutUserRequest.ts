import { ClientRequest } from '@/usecases/common/client';
import { signOutUserResponseSchema, type SignOutUserResponseSchema } from '../common';
import type { ApiResponse } from '@/usecases/common/client/ClientRequest';

export async function signOutUserRequest(): Promise<ApiResponse<SignOutUserResponseSchema>> {
	return await ClientRequest.send('/api/auth/sign-out', signOutUserResponseSchema, {
		method: 'POST'
	});
}
