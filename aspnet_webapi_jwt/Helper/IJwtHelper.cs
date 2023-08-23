using aspnet_webapi_jwt.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace aspnet_webapi_jwt.Helper
{
    public interface IJwtHelper
    {
        string GenerateToken(string userName, int expireMinutes = 120);
        JwtSecurityToken GetToken(List<Claim> authClaims);

        User ValidateToken(string token);
    }
}