using Shared;

namespace AuthManager.Application.UsesCases.LoginCase;

public interface ILoginUseCase : IApplicationContract<LoginCredentials, LoginUserData>;