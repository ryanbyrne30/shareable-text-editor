import { ClientRequest, type ApiResponse } from '@/usecases/common/client/ClientRequest';
import { getCurrentUserResponseSchema, type GetCurrentUserResponseSchema } from '../common';

export type GetCurrentUserResult = ApiResponse<GetCurrentUserResponseSchema>;

export async function getCurrentUserRequest(): Promise<GetCurrentUserResult> {
	return await ClientRequest.send('/api/auth/current', getCurrentUserResponseSchema);
}
