import type { RequestEvent } from '@sveltejs/kit';
import type { z, ZodType } from 'zod';
import {
	BadRequestError,
	InternalServerError,
	UnauthenticatedRequestError,
	UnauthorizedRequestError
} from './errors';
import { Logger } from './Logger';
import { HttpStatusCode } from '@/usecases/common/common/HttpStatusCode';
import { errorResponseSchema, type ErrorResponseSchema } from '../common';
import { Formatter } from './Formatter';

type Promisable<T> = T | Promise<T>;
export type EndpointHandler = (event: RequestEvent) => Promisable<Response>;
export type Middleware = (event: RequestEvent) => Promisable<Response> | Promisable<void>;

type EndpointOptions = {
	middleware: Middleware[];
};

export class Endpoint {
	public static new = (handler: EndpointHandler, options?: EndpointOptions): EndpointHandler => {
		return async (event: RequestEvent) => {
			try {
				const middlewares = options?.middleware ?? [];
				for (let m of middlewares) {
					const response = await m(event);
					if (response) return response;
				}
				return await handler(event);
			} catch (error) {
				return this.exceptionHandler(error);
			}
		};
	};

	private static exceptionHandler = (error: unknown): Response => {
		let response: ErrorResponseSchema = {
			status: HttpStatusCode.InternalServerError,
			message: 'Internal server error'
		};

		if (error instanceof BadRequestError) {
			response.status = error.status;
			response.message = error.message;
			response.errors = error.errors;
		} else if (error instanceof InternalServerError) {
			Logger.error({ message: error.message, exception: error.childError });
			error.status = error.status;
		} else {
			Logger.error({ message: 'Unhandled error', exception: error });
		}
		return Endpoint.jsonResponse(response, { status: response.status });
	};

	public static jsonResponse = (o: unknown, options?: ResponseInit): Response => {
		return new Response(JSON.stringify(o), {
			...options,
			headers: {
				'Content-Type': 'application/json',
				...options?.headers
			}
		});
	};

	public static parseRequest = async <T extends ZodType, R = z.infer<T>>(
		request: Request,
		schema: T
	): Promise<R> => {
		const body = await request.json();
		const parsed = schema.safeParse(body);
		if (!parsed.success) throw new BadRequestError({ parseError: parsed.error });
		return parsed.data;
	};

	public static parseServerResponse = async <T extends ZodType>(
		response: Response,
		schema: T
	): Promise<z.infer<T>> => {
		const body = await response.json();
		const parsed = schema.safeParse(body);
		if (!parsed.success)
			throw new InternalServerError('Unexpected response from server', parsed.error);
		return parsed.data;
	};

	public static sendJsonRequest = async <T extends ZodType>(
		url: string,
		responseSchema: T,
		options?: Omit<RequestInit, 'body'> & { body?: unknown }
	): Promise<z.infer<T>> => {
		const body = options?.body ? JSON.stringify(Formatter.toSnakeCase(options.body)) : undefined;
		const response = await fetch(url, {
			...options,
			body,
			headers: {
				'Content-Type': 'application/json',
				...options?.headers
			}
		});

		if (HttpStatusCode.isSuccess(response.status)) {
			return await this.parseServerResponse(response, responseSchema);
		}
		await this.handleUnexpectedResponse(response);
	};

	private static handleUnexpectedResponse = async (response: Response) => {
		if (response.status === HttpStatusCode.Forbidden) throw new UnauthorizedRequestError();
		if (response.status === HttpStatusCode.Unauthorized) throw new UnauthenticatedRequestError();
		return this.handleUnexpectedJsonResponse(response);
	};

	private static handleUnexpectedJsonResponse = async (response: Response) => {
		const data = await response.json();
		const parsed = errorResponseSchema.omit({ status: true }).safeParse(data);
		if (!parsed.success)
			throw new InternalServerError('Unexpected error response from backend', parsed.error);
		throw new BadRequestError({
			message: parsed.data.message,
			errors: parsed.data.errors,
			status: response.status
		});
	};
}
