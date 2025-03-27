using AuthManager.Application.UsesCases.LoginCase;
using Riok.Mapperly.Abstractions;

namespace AuthManager.GRPC.Mapper;

[Mapper]
public static partial class LoginMapper
{
    public static partial LoginCredentials RequestToLoginCredentials(LoginRequest loginRequest);
}