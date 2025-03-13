using System.Security.Claims;

namespace Shared;

public interface IJwtTokenManagement
{
    string Create(IEnumerable<Claim> claims);
    IEnumerable<Claim> Read(string accessToken);
}