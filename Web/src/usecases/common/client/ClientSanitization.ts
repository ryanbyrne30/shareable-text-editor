import { default as DomPurify } from 'dompurify';

export class ClientSanitization {
	public static sanitizeHtml = (html: string): string => {
		return DomPurify.sanitize(html);
	};
}
