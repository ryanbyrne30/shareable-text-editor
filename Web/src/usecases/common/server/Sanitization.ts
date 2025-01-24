import { default as createDomPurify } from 'dompurify';
import { JSDOM } from 'jsdom';

export class Sanitization {
	public static sanitizeHtml = (html: string): string => {
		const window = new JSDOM('').window;
		const DomPurify = createDomPurify(window);
		return DomPurify.sanitize(html);
	};
}
