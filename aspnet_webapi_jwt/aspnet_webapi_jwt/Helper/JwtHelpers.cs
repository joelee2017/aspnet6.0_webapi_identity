using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aspnet_webapi_jwt.Helper
{
    public class JwtHelper
    {
        private readonly JwtSettingsOptions _settings;
        private readonly IConfiguration _configuration;

        public JwtHelper(IOptionsMonitor<JwtSettingsOptions> settings , IConfiguration configuration)
        {
            //注入appsetting的json
            _settings = settings.CurrentValue;
            _configuration = configuration;
        }

        public string GenerateToken(string userName, int expireMinutes = 120)
        {
            //發行人
            var issuer = _settings.ValidIssuer;
            //加密的key，拿來比對jwt-token沒有
            var signKey = _settings.Secret;
            //建立JWT - Token
            var token = JwtBuilder.Create()
                        //所採用的雜湊演算法
                        .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                                                                  //加密key
                        .WithSecret(signKey)
                        //角色
                        .AddClaim("roles", "admin")
                        //JWT ID
                        .AddClaim("jti", Guid.NewGuid().ToString())
                        //發行人
                        .AddClaim("iss", issuer)
                        //使用對象名稱
                        .AddClaim("sub", userName) // User.Identity.Name
                                                   //過期時間
                        .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds())
                        //此時間以前是不可以使用
                        .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //發行時間
                        .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //使用者全名
                        .AddClaim(ClaimTypes.Name, userName)
                        //進行編碼
                        .Encode();
            return token;
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                //audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
    //將appsetting轉為強行別所使用
    public class JwtSettingsOptions
    {
        public string ValidIssuer { get; set; } = "";
        public string Secret { get; set; } = "";
    }
}
