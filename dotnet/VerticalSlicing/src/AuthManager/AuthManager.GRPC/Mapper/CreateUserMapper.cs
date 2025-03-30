using AuthManager.Application.UsesCases.CreateUserCase;
using Riok.Mapperly.Abstractions;

namespace AuthManager.GRPC.Mapper;

[Mapper]
public static partial class CreateUserMapper
{
    public static partial CreateUserData ToUserData(this SignUpRequest signUpRequest);
}