import { backendServer, Endpoint } from '@/usecases/common/server';
import type { Cookies } from '@sveltejs/kit';
import { z } from 'zod';
import type { DeleteDocumentResponseSchema } from '../common';

const expectedResponseSchema = z.object({ message: z.string() });

export async function serverDeleteDocumentRequest(
	cookies: Cookies,
	id: string
): Promise<DeleteDocumentResponseSchema> {
	await backendServer.sendAuthorizedRequest(
		cookies,
		`/api/v1/documents/${id}`,
		expectedResponseSchema,
		{
			method: 'DELETE'
		}
	);
	return { message: 'ok' };
}
