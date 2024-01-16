using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeSpace.Domain.entities;
using SafeSpace.Infrastructure.Helpers;

namespace SafeSpace.Infrastructure.Data
{
    public class SafeSpaceDbContext : IdentityDbContext<User>
    {
        public SafeSpaceDbContext(DbContextOptions<SafeSpaceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.RenameIdentityTables();
        }
    }
}
