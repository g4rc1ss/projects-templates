using AuthManager.Application.Contracts.InfraestructureContracts;
using Infraestructure.Database.Entities;
#if ((UseJwt || UseIdentity) && SqlDatabase)
using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endif

namespace AuthManager.Infraestructure.Repositories;

public class JwtRepository(

#if ((UseJwt || UseIdentity) && SqlDatabase)
    DatabaseContext dbContext
#endif
) : IJwtRepository
{
    public async Task<bool> RemoveTokenByIdsAsync(int userId, string refreshTokenId)
    {
        UserJwtTokensEntity jwtEntity = new()
        {
            UserId = userId,
            Id = refreshTokenId
        };
#if ((UseJwt || UseIdentity) && SqlDatabase)
        dbContext.UserJwtTokens.Remove(jwtEntity);
        return (await dbContext.SaveChangesAsync()) > 0;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<string> CreateTokenAsync(int userId, DateTime expiration)
    {
        UserJwtTokensEntity refreshTokenEntity = new()
        {
            UserId = userId,
            ExpirationUtc = expiration,
        };
#if ((UseJwt || UseIdentity) && SqlDatabase)
        EntityEntry<UserJwtTokensEntity> entity = await dbContext.UserJwtTokens
            .AddAsync(refreshTokenEntity);
        await dbContext.SaveChangesAsync();

        return entity.Entity.Id;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<DateTime> GetTokenExpirationAsync(string refreshTokenId)
    {
#if ((UseJwt || UseIdentity) && SqlDatabase)
        UserJwtTokensEntity refreshTokens = await dbContext.UserJwtTokens
            .SingleAsync(x => x.Id == refreshTokenId);

        return refreshTokens.ExpirationUtc;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<IEnumerable<JwtTokenData>> GetAllTokensByUserId(int userId)
    {
#if ((UseJwt || UseIdentity) && SqlDatabase)
        List<UserJwtTokensEntity> tokenList = await dbContext.UserJwtTokens
            .Where(x => x.UserId == userId && x.ExpirationUtc >= DateTime.UtcNow)
            .ToListAsync();

        return tokenList.Select(x => new JwtTokenData(Guid.Parse(x.Id), x.ExpirationUtc));
#else
        throw new NotImplementedException();
#endif
    }
}