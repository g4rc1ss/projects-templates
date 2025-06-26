using Infraestructure.Auth.IdentityAuth.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Auth.IdentityAuth;

public class EmailSender : IEmailSender<IdentityUserEntity>
{
    public Task SendConfirmationLinkAsync(
        IdentityUserEntity user,
        string email,
        string confirmationLink
    )
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(IdentityUserEntity user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(IdentityUserEntity user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }
}
