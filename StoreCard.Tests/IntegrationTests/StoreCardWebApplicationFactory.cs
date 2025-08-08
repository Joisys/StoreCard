using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreCard.Data;

namespace StoreCard.Tests.IntegrationTests
{
    public class StoreCardWebApplicationFactory : WebApplicationFactory<Program>
    {
        public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StoreCardDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add InMemory database for tests
                    services.AddDbContext<StoreCardDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<StoreCardDbContext>();
                    db.Database.EnsureCreated();

                    //Seed some test data
                    db.Users.Add(new Domain.Entities.User
                    {
                        Id = 1,
                        FullName = "Jose Joseph",
                    });
                    db.SaveChanges();
                });
            }
        }
    }
}
