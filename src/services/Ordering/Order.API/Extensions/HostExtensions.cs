using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Order.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryFromAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();


                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating thr database used on context {typeof(TContext).Name}");

                    if (retryFromAvailability < 50)
                    {
                        retryFromAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retryFromAvailability);
                    }
                }

                return host;
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
