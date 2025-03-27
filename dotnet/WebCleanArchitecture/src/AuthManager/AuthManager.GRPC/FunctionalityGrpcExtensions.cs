using AuthManager.Application;
using AuthManager.GRPC.Services;
using AuthManager.GRPC.Services.TokenManagement;
using AuthManager.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AuthManager.GRPC;

public static class FunctionalityGrpcExtensions
{
    public static void InitFunctionalityGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
        builder.Services.AddRepositoryService(builder.Configuration);
    }

    public static void MapFunctionalityGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<RefreshTokenService>();
        routeBuilder.MapGrpcService<RevokeTokenService>();
        routeBuilder.MapGrpcService<TokenListService>();

        routeBuilder.MapGrpcService<EmailService>();
        routeBuilder.MapGrpcService<LoginService>();
        routeBuilder.MapGrpcService<LogoutService>();
        routeBuilder.MapGrpcService<PasswordService>();
        routeBuilder.MapGrpcService<SignUpService>();
        routeBuilder.MapGrpcService<TotpService>();
    }
}