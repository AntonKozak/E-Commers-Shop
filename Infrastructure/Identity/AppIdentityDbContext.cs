using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext<AppUser>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {

    }
    // we should ovveride OnModelCreating to change identity primary id key from string to int
    protected override void OnModelCreating(ModelBuilder builder)
    {
    // Call the base class implementation of OnModelCreating to include default Identity model configuration.
        base.OnModelCreating(builder);
    }
}
