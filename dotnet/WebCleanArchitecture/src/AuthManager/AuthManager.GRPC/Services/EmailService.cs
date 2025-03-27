using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.ErrorsCode;
using Grpc.Core;

namespace AuthManager.GRPC.Services;

public class EmailService(
    IUserManager userManager,
    IEmailManager emailManager
) : Email.EmailBase
{
    public override async Task<ConfirmEmailResponse> ConfirmEmail(ConfirmEmailRequest request,
        ServerCallContext context)
    {
        UserData? user = await userManager.GetUserByUserNameAsync(request.Username);
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, UserManagerErrors.InvalidUserCode));
        }

        bool confirm = await emailManager.ConfirmEmailAsync(user.UserName, request.Token);

        return new ConfirmEmailResponse();
    }
}