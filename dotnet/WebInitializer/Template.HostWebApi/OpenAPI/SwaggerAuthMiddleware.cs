using System.Net.Http.Headers;
using System.Text;

namespace Template.HostWebApi.OpenAPI;

public class SwaggerAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task Invoke(HttpContext context)
    {
        IConfigurationSection swaggerAuth = configuration.GetSection("SwaggerAuth");
        string? userNameValue = swaggerAuth["Username"];
        string? passwordValue = swaggerAuth["Password"];

        if (context.Request.Path.StartsWithSegments("/swagger")
            && !string.IsNullOrWhiteSpace(userNameValue)
            && !string.IsNullOrWhiteSpace(passwordValue))
        {
            string? authHeaderValue = context.Request.Headers["Authorization"];
            if (authHeaderValue is not null && authHeaderValue.StartsWith("Basic "))
            {
                AuthenticationHeaderValue header = AuthenticationHeaderValue.Parse(authHeaderValue);
                byte[] bytes = Convert.FromBase64String(header.Parameter!);

                string[] credentials = Encoding.UTF8.GetString(bytes).Split(':');
                string userName = credentials[0];
                string password = credentials[1];

                if (userName.Equals(userNameValue, StringComparison.Ordinal)
                    && password.Equals(passwordValue, StringComparison.Ordinal))
                {
                    await next.Invoke(context);
                    return;
                }
            }

            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            await next.Invoke(context);
        }
    }
}