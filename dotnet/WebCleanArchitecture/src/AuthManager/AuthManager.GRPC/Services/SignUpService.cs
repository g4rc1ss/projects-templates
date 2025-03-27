using AuthManager.Application.UsesCases.CreateUserCase;
using AuthManager.GRPC.Mapper;
using Grpc.Core;
using Infraestructure.Events;
using Microsoft.Extensions.Logging;
using Shared;

namespace AuthManager.GRPC.Services;

public class SignUpService(
    ILogger<SignUpService> logger,
    ICreateUserUseCase createUser,
    IEventNotificator eventNotificator
) : SignUp.SignUpBase
{
    public override async Task<SignUpResponse> CreateAccount(SignUpRequest request, ServerCallContext context)
    {
        CreateUserData userData = request.ToUserData();

        Result<string> result = await createUser.ExecuteAsync(userData, context.CancellationToken);
        if (!result.IsSuccess)
        {
            // TODO: Ver como devolver codigos de error
            return new SignUpResponse();
        }

        // string? url = Url.Action("ConfirmEmail", "ConfirmEmail", new
        // {
        //     userName = request.UserName,
        //     Token = result.Data,
        // }, Request.Scheme);

        // await eventNotificator.PublishAsync(new ConfirmEmailRequest(request.Email, url), context.CancellationToken);

        // TODO: Ver como devolver codigos OK
        return new SignUpResponse();
    }
}