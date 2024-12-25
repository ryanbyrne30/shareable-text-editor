import type { LoadEvent } from '@sveltejs/kit';

export const ssr = false;

export async function load({ params }: LoadEvent) {
	return {
		id: params.id
	};
}
