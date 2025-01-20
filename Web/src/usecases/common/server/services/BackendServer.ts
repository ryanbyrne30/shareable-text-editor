import type { z, ZodType } from 'zod';
import { Endpoint } from '../Endpoint';

export class BackendServer {
	constructor(private serverUrl: string) {}

	public sendRequest = async <T extends ZodType>(
		endpoint: string,
		responseSchema: T,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<z.infer<T>> => {
		return await Endpoint.sendJsonRequest(this.serverUrl + endpoint, responseSchema, options);
	};
}
