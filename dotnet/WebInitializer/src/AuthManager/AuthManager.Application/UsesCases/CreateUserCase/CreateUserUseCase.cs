using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain.ErrorsCode;
using Shared;

namespace AuthManager.Application.UsesCases.CreateUserCase;

public class CreateUserUseCase(
    IUserManager userManager,
    IEmailManager emailManager
) : ICreateUserUseCase
{
    public async Task<Result<string>> ExecuteAsync(CreateUserData userData,
        CancellationToken cancellationToken = default)
    {
        bool user = await userManager.CreateAsync(userData.UserName, userData.Password, userData.Email);

        if (!user)
        {
            return Result.Failure<string>(new Error(SignUpError.CreateAccountFailedCode,
                SignUpError.CreateAccountFailedDesc));
        }


        return await emailManager.GenerateEmailConfirmationTokenAsync(userData.UserName);
    }
}