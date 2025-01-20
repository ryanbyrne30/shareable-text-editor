import { HttpStatusCode } from '@/usecases/common/server/StatusCodes';
import {
	registerUserResponseSchema,
	type RegisterUserFormSchema,
	type RegisterUserResponseSchema
} from '../common';
import { errorResponseSchema } from '@/usecases/common/common/responses/errorResponse';
import type { ApiResponse } from '@/usecases/common/client/responses';

export type RegisterUserResult = ApiResponse<RegisterUserResponseSchema>;

export async function registerUserRequest(
	input: RegisterUserFormSchema
): Promise<RegisterUserResult> {
	const response = await fetch('/api/auth/register', {
		method: 'POST',
		body: JSON.stringify(input)
	});
	if (HttpStatusCode.isSuccess(response.status)) {
		const data = await response.json();
		return { data: registerUserResponseSchema.parse(data) };
	} else if (HttpStatusCode.isBadRequest(response.status)) {
		const data = await response.json();
		const e = errorResponseSchema.parse(data);
		return {
			error: {
				message: e.message,
				errors: e.errors ?? {}
			}
		};
	} else {
		console.error(await response.json());
		throw new Error('Unexpected response from API: ' + response.status);
	}
}
