using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Shared.JWT;
using System.Globalization;

namespace AuthManager.GRPC.Services.TokenManagement;

[Authorize]
public class TokenListService(
    ILogger<TokenListService> logger,
    IJwtTokenManagement jwtTokenManagement,
    IJwtRepository jwtTokenRepository
) : TokenList.TokenListBase
{
    public override async Task<GetTokensResponse> GetTokens(GetTokensRequest request, ServerCallContext context)
    {
        IEnumerable<JwtTokenData> tokens = await jwtTokenRepository.GetAllTokensByUserId(1);

        GetTokensResponse returnList = new();
        returnList.TokenInfo.AddRange(tokens.Select(x => new TokenInformation
        {
            TokenId = x.TokenId.ToString(),
            Expiration = x.Expiration.ToString(CultureInfo.InvariantCulture),
        }));

        return returnList;
    }
}