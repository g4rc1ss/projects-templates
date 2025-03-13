using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.ConfigurationOptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Template.HostWebApi;

public class JwtTokenManagement(IOptions<JwtOption> jwtOptions)
    : IJwtTokenManagement
{
    public string Create(IEnumerable<Claim> claims)
    {
        SigningCredentials credentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key)),
            SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken securityToken = new(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.Value.ExpireSeconds),
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public IEnumerable<Claim> Read(string accessToken)
    {
        JwtSecurityToken? securityToken = new JwtSecurityTokenHandler()
            .ReadJwtToken(accessToken);

        return securityToken.Claims;
    }
}