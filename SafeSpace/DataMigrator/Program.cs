using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeSpace.Infrastructure.Data;

namespace DataMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create a service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Execute the migration
            ExecuteMigration(serviceProvider);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Add DbContext
            services.AddDbContext<Context>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("SafeSpaceDatabase"),
                new MySqlServerVersion(new Version(8, 0, 34)));
            });
        }

        private static void ExecuteMigration(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();

                Console.WriteLine("Applying migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
        }
    }
}
