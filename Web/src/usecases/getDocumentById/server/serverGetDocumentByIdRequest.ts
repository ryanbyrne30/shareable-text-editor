import { backendServer, BadRequestError } from '@/usecases/common/server';
import type { RequestEvent } from '@sveltejs/kit';
import { z } from 'zod';
import type { GetDocumentByIdResponseSchema } from '../common';

const expectedResponseSchema = z.object({
	id: z.string(),
	name: z.string()
});

export async function serverGetDocumentByIdRequest({
	params,
	cookies
}: RequestEvent): Promise<GetDocumentByIdResponseSchema> {
	const id = params['id'];
	if (!id) throw new BadRequestError({ message: 'Id not given' });
	const serverResposne = await backendServer.sendAuthorizedRequest(
		cookies,
		`/api/v1/documents/${id}`,
		expectedResponseSchema
	);

	return {
		id: serverResposne.id,
		name: serverResposne.name
	};
}
