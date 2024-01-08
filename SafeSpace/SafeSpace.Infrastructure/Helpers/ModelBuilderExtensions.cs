
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeSpace.Domain.entities;

namespace SafeSpace.Infrastructure.Helpers
{
    public static class ModelBuilderExtensions
    {
        public static void RenameIdentityTables(this ModelBuilder builder)
        {
            builder.Entity<User>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("User_Roles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("User_Claims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("User_Logins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("Role_Claims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("User_Tokens"); });
        }
    }
}
