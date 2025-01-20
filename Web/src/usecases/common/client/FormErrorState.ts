import { writable, type Writable } from 'svelte/store';
import type { z, ZodType } from 'zod';

export class FormErrorState<T extends ZodType> {
	private _errors: Partial<Record<keyof z.infer<T>, string[] | undefined>> = {};
	private validated: z.infer<T> | null = null;
	public errors: Writable<Partial<Record<keyof z.infer<T>, string[] | undefined>>> = writable({});

	constructor(public checker: T) {}

	public reset = () => {
		this._errors = {};
		this.errors.set({});
	};

	public validate = (data: Record<string, FormDataEntryValue>) => {
		const parsed = this.checker.safeParse(data);
		if (parsed.success) {
			this.validated = parsed.data;
			return;
		}
		const errors = parsed.error.formErrors.fieldErrors;
		this._errors = { ...this._errors, ...errors };
		this.errors.set({ ...this._errors });
	};

	public addError = (key: keyof z.infer<T>, message: string): FormErrorState<T> => {
		if (this._errors[key] !== undefined) this._errors[key].push(message);
		else this._errors[key] = [message];
		this.errors.set({ ...this._errors });
		return this;
	};

	public isValid = (): boolean => {
		for (let v of Object.values(this._errors)) {
			if (v?.length) return false;
		}
		return true;
	};

	public getValidData = (): z.infer<T> => {
		if (this.validated === null)
			throw new Error('Form has not been validated yet. Did you call validate()?');
		return this.validated;
	};
}
