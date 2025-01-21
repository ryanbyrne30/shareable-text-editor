using System.Xml;
using BackendService.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackendService.Common.Filters;

public class ValidationFilter: IActionFilter
{
    private List<string> _ignoreKeys = ["$", "request"]; 
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;

        var parameters = context.ModelState.Where(ms => ms.Value?.Errors.Count > 0 && !_ignoreKeys.Contains(ms.Key)).ToList();

        var errors = new Dictionary<string, List<string>>();

        foreach (var param in parameters)
        {
            var key = SnakeCase(param.Key);
            var es = param.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? [];
            errors.Add(key, es);
        }
        
        var response = new ErrorResponse
        {
            Message = "Bad request",
            Errors = errors 
        };
        context.Result = new BadRequestObjectResult(response);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private static string SnakeCase(string input)
    {
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }
}