using System.Net;
using System.Text.Json;
using BackendService.Common.Exceptions;
using BackendService.Common.Responses;

namespace BackendService.Common.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            await HandleException(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Internal error occured: {message}", ex.Message);
            await HandleException(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task HandleException(HttpContext context, HttpStatusCode statusCode, string message)
    {
        var response = new ErrorResponse
        {
            Message = message
        };
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(response); 
    }
    
}