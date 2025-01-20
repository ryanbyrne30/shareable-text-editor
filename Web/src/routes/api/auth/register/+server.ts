import { Endpoint } from '@/usecases/common/server';
import { handleRegisterUser } from '@/usecases/registerUser/server/handleRegisterUser';

export const POST = Endpoint.new(handleRegisterUser);
