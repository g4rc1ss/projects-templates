using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared;

namespace Template.HostWebApi.FilterControllers;

public class ResultResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult { Value: Result result })
        {
            return;
        }

        ObjectResult contextResult = context.Result as ObjectResult ?? new ObjectResult(result);
        if (result is { IsSuccess: true })
        {
            contextResult.Value = result.Data;
        }
        else
        {
            contextResult.Value = new ProblemDetails
            {
                Title = result?.Error?.Code,
                Detail = result?.Error?.Message,
                Status = contextResult.StatusCode,
                Instance = context.HttpContext.Request.Path,
            };
        }
    }
}