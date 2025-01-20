import {
	registerUserResponseSchema,
	type RegisterUserFormSchema,
	type RegisterUserResponseSchema
} from '../common';
import { ClientRequest, type ApiResponse } from '@/usecases/common/client/ClientRequest';

export type RegisterUserResult = ApiResponse<RegisterUserResponseSchema>;

export async function registerUserRequest(
	input: RegisterUserFormSchema
): Promise<RegisterUserResult> {
	return await ClientRequest.send('/api/auth/register', registerUserResponseSchema, {
		method: 'POST',
		body: input
	});
}
