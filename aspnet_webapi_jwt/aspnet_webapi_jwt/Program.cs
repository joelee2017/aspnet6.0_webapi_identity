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
builder.Services.AddControllers();
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
        Description = "JWT���Ҵy�z"
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});
#endregion

//�M���w�]�M�g
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//���UJwtHelper
builder.Services.AddSingleton<JwtHelper>();
//�ϥοﶵ�Ҧ����U
builder.Services.Configure<JwtSettingsOptions>(builder.Configuration.GetSection("JwtSettings"));
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
// ���� WeatherForecastController > Login
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        RoleClaimType = "roles",
//        ValidateIssuer = true,
//        ValidateAudience = false,
//        //ValidAudience = configuration["JwtSettings:ValidAudience"],
//        ValidIssuer = configuration["JwtSettings:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
//    };
//});

// ���� WeatherForecastController > Login2
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // �i�H��[Authorize]�P�_����
        RoleClaimType = "roles",
        // �w�]�|�{�ҵo��H
        ValidateIssuer = true,
        ValidIssuer = configuration["JwtSettings:ValidIssuer"],
        // ���{�ҨϥΪ�
        ValidateAudience = false,
        // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
        ValidateIssuerSigningKey = true,
        // ñ���ҨϥΪ�key
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

app.MapControllers();

app.Run();
