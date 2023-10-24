using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VidifyStream.API.Middleware;
using VidifyStream.Data;
using VidifyStream.Data.Context;
using VidifyStream.Data.Models;
using VidifyStream.Logic;
using VidifyStream.Logic.CQRS.Auth.Common;
using VidifyStream.Logic.CQRS.Behaviors;
using VidifyStream.Logic.Hubs;
using VidifyStream.Logic.Mapping;
using VidifyStream.Logic.Services.Files;
using VidifyStream.Logic.Services.Notifications;
using VidifyStream.Logic.Services.Users;
using VidifyStream.Logic.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor to access HttpContext from outer services.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddSingleton<AppData>();

//Add MediatR
builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssembly(typeof(ILogicAssemblyMarker).Assembly));
builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IFileService, LocalFileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserService>();

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
builder.Services.AddAuthentication(AuthScheme.Default)
                .AddCookie(AuthScheme.Default);
// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin-only", policy => 
    {
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes(AuthScheme.Default)
              .AddRequirements(new StatusRequirement(Status.Admin));
    });
    options.AddPolicy("user+", policy =>
    {
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes(AuthScheme.Default)
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
builder.Services.AddSwaggerGen();

// Add Razor Pages
builder.Services.AddRazorPages();

// App configurations
var app = builder.Build();

// Use Swagger in development
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFileServer();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

// Register the middleware
app.UseAuthentication();
app.UseAuthorization();

// Register SignalR hubs
app.MapHub<NotificationsHub>("/wss/notifications");

app.MapHealthChecks("/health");

// Apply pending ef core migrations
app.ApplyPendingMigrations();

// Add debug endpoints
app.MapPost("/debug/addadmin", async (IUserService userService) =>
{
    return await userService.AddAdminDebug();
});

app.Run();
