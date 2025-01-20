export type Log = {
	message?: string;
	errors?: Record<string, string[] | undefined>;
	exception?: unknown;
};

export class Logger {
	public static error = (log: Log) => {
		console.error(log);
	};
}
