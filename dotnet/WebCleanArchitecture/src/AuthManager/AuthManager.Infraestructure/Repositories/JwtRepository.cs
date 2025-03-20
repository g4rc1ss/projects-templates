namespace AuthManager.Infraestructure.Repositories;

public class JwtRepository(
) : IJwtRepository
{
    public Task<bool> RemoveTokenByIdsAsync(int userId, string refreshTokenId)
    {
        throw new NotImplementedException();
    }

    public Task<string> CreateTokenAsync(int userId, DateTime expiration)
    {
        throw new NotImplementedException();
    }

    public Task<DateTime> GetTokenExpirationAsync(string refreshTokenId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<JwtTokenData>> GetAllTokensByUserId(int userId)
    {
        throw new NotImplementedException();
    }
}