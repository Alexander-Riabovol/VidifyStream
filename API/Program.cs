using Data.Context;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add Data Protection which will help to secure Autentication process.
builder.Services.AddDataProtection();
// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    assembly => assembly.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
});

// Configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use((ctx, next) =>
{
    if (ctx.Request.Cookies.ContainsKey("auth"))
    {
        var ddp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
        var protector = ddp.CreateProtector("auth-cookie");

        var protectedPayload = ctx.Request.Cookies["auth"];
        var payload = protector.Unprotect(protectedPayload!);
        var parts = payload.Split(':');
        var key = parts[0];
        var value = parts[1];

        var claims = new List<Claim>();
        claims.Add(new Claim(key, value));
        var identity = new ClaimsIdentity(claims);
        ctx.User = new ClaimsPrincipal(identity);
    }

    return next();
});

// debug
app.MapGet("/username", (HttpContext ctx) =>
{
    return ctx.User.FindFirst("usr")?.Value;
});

app.MapGet("/login", (IAuthService auth) =>
{
    auth.SignIn();
    return "ok";
});

app.Run();
