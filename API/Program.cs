using API.Middleware;
using Data.Context;
using Logic.Mapping;
using Data.Models;
using Logic;
using Logic.Hubs;
using Logic.Services.AuthService;
using Logic.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Logic.Services.CommentService;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddSingleton<AppData>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IAuthorizationHandler, StatusRequirementHandler>();

// Add DB context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Docker"),
    // Set Data project as source for migrations
    assembly =>
    {
        assembly.MigrationsAssembly(typeof(DataContext).Assembly.FullName);
        assembly.EnableRetryOnFailure();
    });
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
    options.AddPolicy("user+", policy =>
    {
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes(IAuthService.AuthScheme)
              .AddRequirements(new StatusRequirement(Status.User, Status.Janitor, Status.Admin));
    });
});

// Add Mapster configurations
builder.Services.AddMappings();

// Add Health Checks
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddSignalR();

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

app.MapControllers();

// Register the middleware
app.UseAuthentication();
app.UseAuthorization();

// Register SignalR hubs
app.MapHub<NotificationsHub>("/wss/notifications");

// Map the testing endpoints
app.MapGet("/username", (HttpContext ctx) =>
{
    return $"{ctx.User.FindFirst("id")?.Value}";
}).RequireAuthorization("user+");

app.MapGet("/test", () =>
{
    return "Succesfull!";
}).AllowAnonymous();

app.MapHealthChecks("/health");

app.Run();
