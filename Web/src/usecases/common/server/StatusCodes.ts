export class HttpStatusCode {
	public static OK = 200;
	public static Created = 201;
	public static Accepted = 202;
	public static NoContent = 204;
	public static BadRequest = 400;
	public static Unauthorized = 401;
	public static Forbidden = 403;
	public static NotFound = 404;
	public static MethodNotAllowed = 405;
	public static Conflict = 409;
	public static InternalServerError = 500;
	public static NotImplemented = 501;
	public static BadGateway = 502;
	public static ServiceUnavailable = 503;
	public static GatewayTimeout = 504;

	public static isSuccess = (statusCode: number): boolean => {
		return statusCode < 300 && statusCode >= 200;
	};

	public static isBadRequest = (statusCode: number): boolean => {
		return statusCode < 500 && statusCode >= 400;
	};
}
