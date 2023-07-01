using API.Middleware;
using Data.Context;
using Logic.Mapping;
using Data.Models;
using Logic;
using Logic.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Logic.Services.AuthService;
using Logic.Services.CommentService;
using Logic.Services.FileService;
using Logic.Services.NotificationService;
using Logic.Services.UserService;
using Logic.Services.ValidationService;
using Logic.Services.VideoService;
using FluentValidation;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddSingleton<AppData>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFileService, LocalFileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVideoService, VideoService>();

builder.Services.AddScoped<IAuthorizationHandler, StatusRequirementHandler>();

// Add DB context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(AppData.InDocker ? "Docker" : "Default"),
    // Set Data project as source for migrations
    assembly =>
    {
        assembly.MigrationsAssembly(typeof(IDataAssemblyMarker).Assembly.FullName);
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

// Add Validation
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddValidatorsFromAssemblyContaining<ILogicAssemblyMarker>();

// Add Health Checks
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

// Add Razor Pages
builder.Services.AddRazorPages();

// App configurations
var app = builder.Build();

app.UseFileServer();
// Configure the HTTP request pipeline.

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
    return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
}).AllowAnonymous();

app.MapHealthChecks("/health");

// Apply pending ef core migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DataContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
