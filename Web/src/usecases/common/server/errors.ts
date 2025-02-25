import { HttpStatusCode } from '@/usecases/common/common/HttpStatusCode';
import type { z, ZodError } from 'zod';

type ErrorsType = Record<string, string[] | undefined>;

export class CustomError extends Error {
	public errors: Record<string, string[] | undefined>;
	public status: number;

	constructor(message: string, errors: ErrorsType, status: number) {
		super(message);
		this.errors = errors;
		this.status = status;
	}
}

export class BadRequestError extends CustomError {
	constructor(args: {
		message?: string;
		errors?: ErrorsType;
		status?: number;
		parseError?: ZodError;
	}) {
		const message = args.message ?? 'Bad request';

		let errors: ErrorsType = {};
		if (args.errors) errors = args.errors;
		else if (args.parseError) args.errors = args.parseError.formErrors.fieldErrors;

		const status = args.status ?? HttpStatusCode.BadRequest;

		super(message, errors, status);
	}
}

export class UnauthorizedRequestError extends BadRequestError {
	constructor() {
		super({ message: 'Unauthorized', status: HttpStatusCode.Forbidden });
	}
}

export class UnauthenticatedRequestError extends BadRequestError {
	constructor() {
		super({ message: 'Unauthenticated', status: HttpStatusCode.Unauthorized });
	}
}

export class InternalServerError extends Error {
	public status: number = HttpStatusCode.InternalServerError;
	public childError: unknown;

	constructor(message: string, error: unknown) {
		super(message);
		this.childError = error;
	}
}
