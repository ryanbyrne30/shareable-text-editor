import { env } from '../env';
import { BackendServer } from './BackendServer';

export const backendServer = new BackendServer(env.BACKEND_SERVER_URL);
