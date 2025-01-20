import { describe, expect, it } from 'vitest';
import { Endpoint, type EndpointHandler, type Middleware } from '../Endpoint';
import { errorResponseSchema } from '@/usecases/common/common/responses/errorResponse';

describe('endpoint', () => {
	it('should run middleware in correct order', async () => {
		const numMiddleware = 10;
		const expected: number[] = [];
		for (let i = 0; i < numMiddleware; i++) expected.push(i);

		const received: number[] = [];
		const middleware: Middleware[] = [];
		for (let i = 0; i < numMiddleware; i++)
			middleware.push(() => {
				received.push(i);
			});

		const handler: EndpointHandler = () => new Response();
		const e = Endpoint.new(handler, { middleware }) as any;

		await e();

		for (let i = 0; i < numMiddleware; i++) {
			const exp = expected[i];
			const rec = received[i];
			expect(rec).toBe(exp);
		}
	});

	it('should run handle errors properly', async () => {
		const handler: EndpointHandler = () => {
			throw new Error();
		};
		const e = (await Endpoint.new(handler)) as any;

		const response: Response = await e();
		const data = await response.json();
		const parsed = errorResponseSchema.safeParse(data);

		expect(parsed.success).toBeTruthy();
	});
});
