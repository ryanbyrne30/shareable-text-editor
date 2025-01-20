export type ApiResponse<T> = {
	data?: T;
	error?: {
		message: string;
		errors?: Record<string, string[] | undefined>;
	};
};
