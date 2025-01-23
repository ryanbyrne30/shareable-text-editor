import { browser } from '$app/environment';
import { getCurrentUserRequest } from '@/usecases/getCurrentUser/client';
import { writable } from 'svelte/store';
import { z } from 'zod';
import { HttpStatusCode } from '../../common';

const localStorageKey = 'user';

const userStoreSchema = z.object({
	username: z.string()
});
export type UserStore = z.infer<typeof userStoreSchema>;

function getUser(): UserStore | null {
	if (!browser) return null;
	const stored = localStorage.getItem(localStorageKey);
	if (stored === null) return null;

	const parsed = userStoreSchema.safeParse(JSON.parse(stored));
	if (!parsed.success) {
		console.error(parsed.error);
		throw new Error('Could not parse user data from localStorage');
	}

	return parsed.data;
}

const storedUser = getUser();
export const userStore = writable(storedUser);

userStore.subscribe((value) => {
	if (!browser) return;
	if (value === null) localStorage.removeItem(localStorageKey);
	else localStorage.setItem(localStorageKey, JSON.stringify(value));
});

export function updateUserStore() {
	getCurrentUserRequest().then((res) => {
		if (res.error && res.error.status === HttpStatusCode.Unauthorized) {
			userStore.set(null);
			return;
		}
		if (res.error) {
			console.error('Error when updating user store');
			console.error(res.error);
			return;
		}
		const user = res.data;
		if (!user) userStore.set(null);
		else userStore.set({ username: user.username });
	});
}
