using System.Security.Claims;
using System.Text;
using jwtAuth;
using jwtAuth.Extensions;
using jwtAuth.Models;
using jwtAuth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<AuthService>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(x => { x.AddPolicy("admin", p => p.RequireRole("admin")); });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (AuthService service) =>
{
    var user = new User(
        1,
        "bruno.bernardes",
        "Bruno Bernardes",
        "bruno@gmail.com",
        "q1w2e3r4t5",
        ["developer"]);

    return service.Create(user);
});

app.MapGet("/test", () => "OK!")
    .RequireAuthorization();

app.MapGet("/test/user", (ClaimsPrincipal user) => new
    {
        id = user.GetId(),
        name = user.GetName(),
        givenName = user.GetGivenName(),
        email = user.GetEmail(),
    })
    .RequireAuthorization();

app.MapGet("/test/admin", () => "admin OK!")
    .RequireAuthorization("admin");

app.Run();