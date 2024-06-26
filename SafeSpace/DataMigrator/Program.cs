﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeSpace.Domain.entities;
using SafeSpace.Domain.enums;
using SafeSpace.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DbUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create a service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Execute the migration
            await ExecuteMigrationAsync(serviceProvider);

            // Seed the roles
            SeedRoles(serviceProvider);

            //Seed the Admin
            SeedAdmin(serviceProvider);

            Console.WriteLine("Migration and Seeding completed.");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Add DbContext
            services.AddDbContext<SafeSpaceDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("SafeSpaceDatabase"),
                    new MySqlServerVersion(new Version(8, 0, 34)));
            });

            // Add RoleManager if needed
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<SafeSpaceDbContext>()
                .AddUserManager<UserManager<User>>();
        }

        private static async Task ExecuteMigrationAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SafeSpaceDbContext>();

                Console.WriteLine("Applying migrations...");
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Migrations applied successfully.");
            }
        }

        private static void SeedRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                Console.WriteLine("Seeding roles...");

                foreach (var roleName in Enum.GetNames(typeof(UserRole)))
                {
                    if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole { Name = roleName }).GetAwaiter().GetResult();
                    }
                }

                Console.WriteLine("Roles seeded successfully.");
            }
        }
        private static void SeedAdmin(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                Console.WriteLine("Seeding Admin...");

                if (!userManager.Users.AnyAsync(u => u.Email == "safespacesolutions.beta@gmail.com").GetAwaiter().GetResult())
                {
                    var admin = new User()
                    {
                        FirstName = "SafeSpace",
                        LastName = "Admin",
                        UserName = "safespace",
                        Email = "safespacesolutions.beta@gmail.com",
                        Gender = "Other",
                        EmailConfirmed = true,
                    };
                    var password = "Admin@123";
                    userManager.CreateAsync(admin, password).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
                    Console.WriteLine("Admin Seeded Succefully");
                }
             }
        }
    }
}
