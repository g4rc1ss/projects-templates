using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CompletedWeb.API.Endpoints.ValidateEnpoints;

public static class ValidateEndpoints
{
    public static void MapValidateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("validate").MapPostPocValidate();
    }
}