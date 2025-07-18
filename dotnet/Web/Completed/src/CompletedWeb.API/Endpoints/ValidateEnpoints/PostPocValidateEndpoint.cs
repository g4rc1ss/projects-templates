using CompletedWeb.API.Models;
using Garciss.ROP;
using Garciss.ROP.ApiExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompletedWeb.API.Endpoints.ValidateEnpoints;

public static class PostPocValidateEndpoint
{
    public static void MapPostPocValidate(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost(
                "",
                ([FromBody] Request request) =>
                {
                    Result result = new([new Error("prueba", "prueba.message")]);

                    return Task.FromResult(result.ToResults());
                }
            )
            .ProducesValidationProblem();
    }
}
