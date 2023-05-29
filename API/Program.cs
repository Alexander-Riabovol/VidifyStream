using Data.Context;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add Authentication
builder.Services.AddAuthentication(IAuthService.AuthScheme)
                .AddCookie(IAuthService.AuthScheme);

// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    // Set Data project as source for migrations
    assembly => assembly.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
});

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
    return $"{ctx.User.FindFirst("id")?.Value} {ctx.User.FindFirst("status")?.Value}";
});

app.Run();
