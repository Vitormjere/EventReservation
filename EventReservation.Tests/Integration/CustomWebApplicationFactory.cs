using EventReservation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;

namespace EventReservation.Tests.Integration {
    public class CustomWebApplicationFactory : WebApplicationFactory<Program> {
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactory() {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureServices(services =>
            {
                var descriptorsToRemove = services
                    .Where(d => d.ServiceType.Namespace != null &&
                                d.ServiceType.Namespace.StartsWith("Microsoft.EntityFrameworkCore"))
                    .ToList();

                foreach (var descriptor in descriptorsToRemove) {
                    services.Remove(descriptor);
                }

                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbContextDescriptor is not null) {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing) {
            _connection.Close();
            base.Dispose(disposing);
        }
    }
}