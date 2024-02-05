using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services to the container using the extension method.
builder.Services.AddApplicationServices(builder.Configuration);


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

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<StoreContext>>();
try
{
    context.Database.Migrate();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();