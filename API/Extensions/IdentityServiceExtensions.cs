using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
        });

        services.AddIdentityCore<AppUser>(opt =>
        {
            //we can add more options here like password, lockout, user, token, email, phone, and claim options
            opt.Password.RequiredLength = 5;
        })
            .AddEntityFrameworkStores<AppIdentityDbContext>()// create tables for identity in the database
            .AddSignInManager<SignInManager<AppUser>>();// it allows users to authenticate and sign in to the application

        services.AddAuthentication();

        services.AddAuthentication();


        return services;
    }
}
