using API.Middleware;
using Data.Context;
using Data.Models;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthorizationHandler, StatusRequirementHandler>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    // Set Data project as source for migrations
    assembly => assembly.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
});

// Add Authentication
builder.Services.AddAuthentication(IAuthService.AuthScheme)
                .AddCookie(IAuthService.AuthScheme);
// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin-only", policy => 
    {
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes(IAuthService.AuthScheme)
              .AddRequirements(new StatusRequirement(Status.Admin));
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App configurations
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

// Register the middleware
app.UseAuthentication();

// debug
app.MapGet("/username", (HttpContext ctx) =>
{
    if (!ctx.User.Identities.Any(x => x.AuthenticationType == IAuthService.AuthScheme))
    {
        ctx.Response.StatusCode = 401;
        return "Not Authorized";
    }

    if(!ctx.User.HasClaim("status", "Admin"))
    {
        ctx.Response.StatusCode = 403;
        return "Only admins can access this endpoint";
    }

    return $"{ctx.User.FindFirst("id")?.Value} {ctx.User.FindFirst("status")?.Value}";
});

app.Run();
