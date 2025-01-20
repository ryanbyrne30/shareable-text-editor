export class FormUtils {
	public static formData = (e: SubmitEvent): Record<string, FormDataEntryValue> => {
		const form = e.target as HTMLFormElement;
		if (!form) return {};
		return Object.fromEntries(new FormData(form));
	};
}
