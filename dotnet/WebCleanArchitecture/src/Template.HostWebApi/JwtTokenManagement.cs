using AuthManager.Application.Contracts;
using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Template.HostWebApi.ConfigurationOptions;

namespace Template.HostWebApi;

public class JwtTokenManagement(
    ILogger<JwtTokenManagement> logger,
    IJwtRepository jwtRepository,
    IOptions<JwtOption> jwtOptions
) : IJwtTokenManagement
{
    public string Create(JwtData jwtData)
    {
        SigningCredentials credentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key)),
            SecurityAlgorithms.HmacSha256Signature);

        List<Claim> claims =
        [
            new(ClaimsKey.UserId, jwtData.UserId),
            new(ClaimsKey.UserName, jwtData.UserName),
            new(ClaimsKey.UserEmail, jwtData.UserEmail),
            new(ClaimsKey.RefreshToken, jwtData.RefreshToken)
        ];

        if (jwtData.AdditionalClaims != null)
        {
            claims.AddRange(jwtData.AdditionalClaims);
        }

        JwtSecurityToken securityToken = new(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.Value.ExpireSeconds),
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string Refresh(string accessToken)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidIssuer = jwtOptions.Value.Issuer,
            ValidAudience = jwtOptions.Value.Audience,
            IssuerSigningKey = key
        };
        JwtSecurityTokenHandler? tokenHandler = new();
        tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

        JwtSecurityToken? token = tokenHandler.ReadJwtToken(accessToken);

        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken securityToken = new(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.Value.ExpireSeconds),
            claims: token.Claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public IEnumerable<Claim> ReadClaims(string accessToken)
    {
        JwtSecurityToken? securityToken = new JwtSecurityTokenHandler()
            .ReadJwtToken(accessToken);

        return securityToken.Claims;
    }

    public Task<string> CreateRefreshTokenAsync(int userId)
    {
        return jwtRepository.CreateTokenAsync(userId, DateTime.UtcNow.AddMonths(1));
    }

    public Task<bool> RevokeRefreshTokenAsync(int userId, string refreshTokenId)
    {
        return jwtRepository.RemoveTokenByIdsAsync(userId, refreshTokenId);
    }
}