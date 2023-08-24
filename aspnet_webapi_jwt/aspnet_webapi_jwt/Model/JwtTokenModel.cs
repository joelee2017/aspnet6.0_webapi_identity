using Newtonsoft.Json;
using System.Security.Claims;

namespace aspnet_webapi_jwt.Model
{
    public class JwtTokenModel
    {
        //對應 WeatherForecastController > Login2 
        //[JsonProperty(ClaimTypes.Role)]
        public string roles { get; set; }
        public string jti { get; set; }
        public string iss { get; set; }
        public string sub { get; set; }
        public long exp { get; set; }
        public long nbf { get; set; }
        public long iat { get; set; }

        [JsonProperty(ClaimTypes.Name)]
        public string Name { get; set; }

        // 對應 WeatherForecastController > Login 
        [JsonProperty(ClaimTypes.Role)]
        public List<string> Roles { get; set; }
    }
}
