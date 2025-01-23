import { serverGetDocumentByIdRequest } from '@/usecases/getDocumentById/server';
import type { ServerLoadEvent } from '@sveltejs/kit';

export async function load(event: ServerLoadEvent) {
	const document = await serverGetDocumentByIdRequest(event);

	return {
		document
	};
}
