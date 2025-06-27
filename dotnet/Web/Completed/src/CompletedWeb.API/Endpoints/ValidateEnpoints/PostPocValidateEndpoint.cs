using CompletedWeb.API.Models;
using CompletedWeb.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompletedWeb.API.Endpoints.ValidateEnpoints;

public static class PostPocValidateEndpoint
{
    public static void MapPostPocValidate(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("", HandlerAsync);
    }

    private static Task<IResult> HandlerAsync([FromBody] Request request)
    {
        Result result = new([new Error("prueba", "prueba.message")]);
        result = Result.Success();
        result = Result.Failure(new Error("", ""));

        Result errorResult = new Error("", "").ToResult();

        if (result.IsSuccess)
        {
            return Task.FromResult(Results.Ok());
        }

        return Task.FromResult(Results.BadRequest(result.Errors));
    }
}
