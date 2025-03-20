using AuthManager.Domain.BusinessObjects;

namespace AuthManager.Domain.InfraestructureContracts;

public interface IUserManager
{
    Task<UserData?> GetUserByIdAsync(string userId);
    Task<UserData?> GetUserByUserNameAsync(string userName);
    Task<bool> UserExistsAsync(string userName);
    Task<IList<string>> GetUserRolesAsync(int userId);
    Task<bool> IsLockedAsync(string userName);
    Task<bool> CreateAsync(string userName, string password, string email);
}