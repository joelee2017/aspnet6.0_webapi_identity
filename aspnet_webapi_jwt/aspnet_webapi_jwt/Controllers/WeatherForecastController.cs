using aspnet_webapi_jwt.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using aspnet_webapi_jwt.Extension;

namespace aspnet_webapi_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJwtHelper _jwtHelpers;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IJwtHelper jwtHelpers)
        {
            _logger = logger;
            _configuration = configuration;
            _jwtHelpers = jwtHelpers;
        }

        
        [HttpGet(Name = "GetWeatherForecast"), AllowAnonymous]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username != null)
            {
                var userRoles = new List<string>() { "admin", "user" };

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _jwtHelpers.GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost("Login2")]
        public ActionResult<string> Login2(string username, string password)
        {
            var token = _jwtHelpers.GenerateToken(username);
            return Ok(token);
        }
        [HttpGet("username"), Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin")]
        public ActionResult<string> Username()
        {
            return Ok(User.Identity?.Name);
        }
    }
}