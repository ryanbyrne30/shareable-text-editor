import { Endpoint } from '@/usecases/common/server';
import { handleSignOutUser } from '@/usecases/signOutUser/server';

export const POST = Endpoint.new(handleSignOutUser);
