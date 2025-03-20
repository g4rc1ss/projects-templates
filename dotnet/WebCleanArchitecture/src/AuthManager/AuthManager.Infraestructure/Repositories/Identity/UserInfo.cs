using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;

namespace AuthManager.Infraestructure.Repositories.Identity;

internal class UserInfo(
) : IUserInfo
{
    public UserEntity? Entity { get; set; }

    public async Task<UserEntity> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        if (Entity is not null)
        {
            return Entity;
        }

        return default;
    }


    public async Task<UserEntity> GetByIdAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        if (Entity is not null)
        {
            return Entity;
        }

        return default;
    }
}