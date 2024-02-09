using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services to the container using the extension method.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable custom error handling by redirecting to the specified error page for non-success status codes.
app.UseMiddleware<ExeptionMiddleware>();

app.UseStaticFiles();

// Enable custom error handling by redirecting to the specified error page for non-success status codes.
// The {0} placeholder in the specified path will be replaced with the actual status code.
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Enable endpoint routing. from ApplicationServiceExtensions
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManadger = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<StoreContext>>();
try
{
    context.Database.Migrate();
    await identityContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManadger);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();