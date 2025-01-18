using System.Net;

namespace DocumentService.Common.Exceptions;

public class BadRequestException: Exception
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    
    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}