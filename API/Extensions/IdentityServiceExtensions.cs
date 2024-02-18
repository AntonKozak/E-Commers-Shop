using System.Text;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("IdentityConnection"));
        });

        services.AddIdentityCore<AppUser>(opt =>
        {
            //we can add more options here like password, lockout, user, token, email, phone, and claim options
            opt.Password.RequiredLength = 5;
        })
            .AddEntityFrameworkStores<AppIdentityDbContext>()// create tables for identity in the database
            .AddSignInManager<SignInManager<AppUser>>();// it allows users to authenticate and sign in to the application

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false    
                };
            }
        );

        services.AddAuthorization();


        return services;
    }
}
