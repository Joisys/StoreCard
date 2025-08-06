using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StoreCard.Data
{
    public class StoreCardDesignTimeDbContext : IDesignTimeDbContextFactory<StoreCardDbContext>
    {

        public const string ConnectionStringName = "DefaultConnection";
        public IConfiguration Configuration { get; }

        public StoreCardDesignTimeDbContext()
        {
            var basePath = Directory.GetCurrentDirectory();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public StoreCardDbContext CreateDbContext(string[] args)
        {
            string? connectionString = Configuration.GetConnectionString(ConnectionStringName);
            if (connectionString == null)
            {
                throw new InvalidOperationException("Could Not find connection string");
            }
            DbContextOptionsBuilder<StoreCardDbContext> optionsBuilder = new DbContextOptionsBuilder<StoreCardDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new StoreCardDbContext(optionsBuilder.Options);
        }

    }
}
