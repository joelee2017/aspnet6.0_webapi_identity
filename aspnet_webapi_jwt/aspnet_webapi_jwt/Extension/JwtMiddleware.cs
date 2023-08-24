using aspnet_webapi_jwt.Helper;

namespace aspnet_webapi_jwt.Extension;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtHelper _jwtHelper;

    public JwtMiddleware(RequestDelegate next, IJwtHelper jwtHelper)
    {
        _next = next;
        _jwtHelper = jwtHelper;
    }
    //客製化 Middleware 驗證&&解析
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = !string.IsNullOrEmpty(token) ? _jwtHelper.ValidateToken(token) : null;
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = "123";
        }

        await _next(context);
    }
}