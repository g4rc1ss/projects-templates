using System.ComponentModel.DataAnnotations;

namespace CompletedWeb.HostWebApi.Extensions;

public class EndpointValidatorFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        foreach (object? argument in context.Arguments)
        {
            if (argument is null)
            {
                continue;
            }

            ValidationContext validationContext = new(argument);
            List<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(
                argument,
                validationContext,
                validationResults,
                true
            );

            if (!isValid)
            {
                return Results.ValidationProblem(
                    validationResults.ToDictionary(
                        e => e.MemberNames.FirstOrDefault() ?? "General",
                        e => new[] { e.ErrorMessage ?? "Error de validación" }
                    )
                );
            }
        }

        return await next(context);
    }
}
