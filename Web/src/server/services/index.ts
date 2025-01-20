import { env } from '@/usecases/common/server';
import { BackendServer } from './BackendServer';

export const backendServer = new BackendServer(env.BACKEND_SERVER_URL);
