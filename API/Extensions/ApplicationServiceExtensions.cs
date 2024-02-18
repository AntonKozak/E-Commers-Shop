using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<StoreContext>(opt =>{
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddSingleton<IResponseCacheService, ResponceCacheService>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //  custom error handling for API behavor options
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                // Extract error messages from the ModelState.
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                // Create a custom response containing the validation errors.
                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(errorResponse);
            };
        });

                    services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:4200");
                });
            });


        return services;
    }
}
