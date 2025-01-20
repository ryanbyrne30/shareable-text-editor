import { ClientRequest, type ApiResponse } from '@/usecases/common/client';
import {
	signInUserResponseSchema,
	type SignInUserFormSchema,
	type SignInUserResponseSchema
} from '../common';

export type SignInUserResult = ApiResponse<SignInUserResponseSchema>;

export async function signInUserRequest(input: SignInUserFormSchema): Promise<SignInUserResult> {
	return await ClientRequest.send('/api/auth/sign-in', signInUserResponseSchema, {
		method: 'POST',
		body: input
	});
}
