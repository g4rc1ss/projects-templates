using Infraestructure.Database.Entities;

namespace Infraestructure.Database.Repository;

public class UserInfo(

#if (UseIdentity)
    UserManager<UserEntity> userManager
#endif
) : IUserInfo
{
    public UserEntity? Entity { get; set; }

    public async Task<UserEntity?> GetByIdAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (Entity is not null)
        {
            return Entity;
        }
#if (UseIdentity)
        return await userManager.FindByIdAsync(userId);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        if (Entity is not null)
        {
            return Entity;
        }
#if (UseIdentity)
        return await userManager.FindByNameAsync(userName);
#else
        throw new NotImplementedException();
#endif
    }
}

public interface IUserInfo : IRepository<UserEntity>
{
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
}