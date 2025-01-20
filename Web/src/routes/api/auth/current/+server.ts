import { Endpoint } from '@/usecases/common/server';
import { handleGetCurrentUser } from '@/usecases/getCurrentUser/server';

export const GET = Endpoint.new(handleGetCurrentUser);
