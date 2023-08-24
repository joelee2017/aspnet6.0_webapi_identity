using aspnet_webapi_jwt.Extension;
using aspnet_webapi_jwt.Filter;
using aspnet_webapi_jwt.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers(x => x.Filters.Add<AuthorizeAttribute>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Swagger
builder.Services.AddSwaggerGen(options => {
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT驗證描述"
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});
#endregion

//清除預設映射
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//註冊JwtHelper
builder.Services.AddSingleton<IJwtHelper,JwtHelper>();
//使用選項模式註冊
builder.Services.Configure<JwtSettingsOptions>(builder.Configuration.GetSection("JwtSettings"));
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
// 對應 WeatherForecastController > Login
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
        //對應 WeatherForecastController > Login2 
        RoleClaimType = "roles",
        //對應 WeatherForecastController > Login 
        //RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        // 預設會認證發行人
        ValidateIssuer = true,
        // 不認證使用者
        ValidateAudience = false,
        //ValidAudience = configuration["JwtSettings:ValidAudience"],
        ValidIssuer = configuration["JwtSettings:ValidIssuer"],
        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
        ValidateIssuerSigningKey = true,
        // 簽章所使用的key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
