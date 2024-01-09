using jwtAuth.Models;
using jwtAuth.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<AuthService>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (AuthService service) =>
{
    var user = new User(
        1,
        "bruno.bernardes",
        "Bruno Bernardes",
        "bruno@gmail.com",
        "q1w2e3r4t5",
        ["admin","developer"]);
    
    return service.Create(user);
});

app.Run();