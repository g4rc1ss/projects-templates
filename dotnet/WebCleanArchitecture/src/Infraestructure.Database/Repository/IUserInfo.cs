using Infraestructure.Database.Entities;

namespace Infraestructure.Database.Repository;

public interface IUserInfo : IRepository<UserEntity>
{
    Task<UserEntity> GetUserByUserNameAsync(string userName);
}