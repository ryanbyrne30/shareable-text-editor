using System.Net;

namespace BackendService.Common.Exceptions;

public class UnauthorizedRequestException(): BadRequestException("Unauthorized", HttpStatusCode.Forbidden)
{
}