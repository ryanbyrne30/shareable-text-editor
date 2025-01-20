import { z, ZodType } from 'zod';
import { errorResponseSchema, HttpStatusCode } from '../common';

export type ApiResponse<T> = {
	data?: T;
	error?: {
		message: string;
		errors?: Record<string, string[] | undefined>;
	};
};

export class ClientRequest {
	public static send = async <T extends ZodType, R = z.infer<T>>(
		endpoint: string,
		responseSchema: T,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<ApiResponse<R>> => {
		const body = options?.body ? JSON.stringify(options.body) : undefined;

		const response = await fetch(endpoint, {
			...options,
			body: body,
			headers: {
				'Content-Type': 'application/json',
				...options?.headers
			}
		});
		if (HttpStatusCode.isSuccess(response.status)) {
			const data = await response.json();
			return { data: responseSchema.parse(data) };
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
	};
}
