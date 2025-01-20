import { Endpoint } from '@/server/Endpoint';
import { handleRegisterUser } from '@/usecases/registerUser/server/handleRegisterUser';

export const POST = Endpoint.new(handleRegisterUser);
