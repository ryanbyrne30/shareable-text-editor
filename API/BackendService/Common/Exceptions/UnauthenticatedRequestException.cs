using System.Net;

namespace BackendService.Common.Exceptions;

public class UnauthenticatedRequestException(): BadRequestException("Unauthorized", HttpStatusCode.Unauthorized) 
{
}