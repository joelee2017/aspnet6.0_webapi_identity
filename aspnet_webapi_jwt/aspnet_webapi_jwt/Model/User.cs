namespace aspnet_webapi_jwt.Model
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
