export class Formatter {
	public static isRecord = (data: unknown): data is Record<string, unknown> => {
		return Object.prototype.toString.call(data) === '[object Object]';
	};

	public static toSnakeCase = (data: unknown): unknown => {
		if (Array.isArray(data)) {
			for (let i = 0; i < data.length; i++) {
				data[i] = Formatter.toSnakeCase(data[i]);
			}
			return data;
		}
		if (Formatter.isRecord(data)) {
			for (let k of Object.keys(data)) {
				const value = data[k];
				delete data[k];
				const newKey = Formatter.stringToSnakeCase(k);
				data[newKey] = Formatter.toSnakeCase(value);
			}
			return data;
		}
		return data;
	};

	public static stringToSnakeCase = (s: string) => {
		let newString = '';
		for (let i = 0; i < s.length; i++) {
			if (Formatter.isUppercase(s[i]) && i > 0) {
				newString += '_';
			}
			newString += s[i].toLowerCase();
		}

		return newString;
	};

	public static isUppercase = (s: string) => {
		return s.toUpperCase() === s && s.toLowerCase() !== s;
	};

	public static deepEqual = (obj1: any, obj2: any): boolean => {
		if (typeof obj1 !== typeof obj2) return false;
		if (Array.isArray(obj1) && Array.isArray(obj2)) {
			if (obj1.length !== obj2.length) return false;
			for (let i = 0; i < obj1.length; i++) {
				if (!Formatter.deepEqual(obj1[i], obj2[i])) return false;
			}
			return true;
		}

		if (Formatter.isRecord(obj1) && Formatter.isRecord(obj2)) {
			const keys1 = Object.keys(obj1);
			const keys2 = Object.keys(obj2);
			if (keys1.length !== keys2.length) return false;
			for (let k of keys1) {
				if (!Formatter.deepEqual(obj1[k], obj2[k])) return false;
			}
			return true;
		}

		return obj1 === obj2;
	};
}
