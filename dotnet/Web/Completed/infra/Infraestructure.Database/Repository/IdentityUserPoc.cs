using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Database.Repository;

public class IdentityUserPoc(UserManager<IdentityUserEntity> userManager) : IIdentityUserRepository
{
    public IdentityUserEntity? Entity { get; set; }

    public async Task<IdentityUserEntity?> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null)
        {
            return Entity;
        }
        Entity = await userManager.FindByIdAsync(userId);
        return Entity;
    }

    public async Task<IdentityUserEntity> CreateAsync(
        IdentityUserEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        IdentityResult result = await userManager.CreateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error creating user");
        }

        Entity = entity;
        return entity;
    }

    public async Task UpdateAsync(
        IdentityUserEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        IdentityResult result = await userManager.UpdateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error updating user");
        }
    }

    public async Task DeleteAsync(
        IdentityUserEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        IdentityResult result = await userManager.DeleteAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error updating user");
        }
    }
}

public interface IIdentityUserRepository : IRepository<IdentityUserEntity, string>;
