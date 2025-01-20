import { Endpoint } from '@/usecases/common/server';
import { handleSignInUser } from '@/usecases/signInUser/server';

export const POST = Endpoint.new(handleSignInUser);
