using Newtonsoft.Json;

namespace aspnet_webapi_jwt.Model
{
    public class JwtTokenModel
    {      
        public string roles { get; set; }
        public string jti { get; set; }
        public string iss { get; set; }
        public string sub { get; set; }
        public long exp { get; set; }
        public long nbf { get; set; }
        public long iat { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")]
        public string Name { get; set; }

        [JsonProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
        public List<string> Roles { get; set; }
    }
}
