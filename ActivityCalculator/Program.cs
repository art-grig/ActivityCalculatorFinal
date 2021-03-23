using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            if (args.Any(x =>
                    string.Compare(x, "migrate", StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                var loggerFac = host.Services.GetService<ILoggerFactory>();
                var logger = loggerFac.CreateLogger("Migration");
                logger.LogInformation("Migration started");

                var serviceProvider = host.Services.GetService(typeof(IServiceCollection)) as IServiceCollection;
                var dbContexts =
                    serviceProvider.Where(x => x.ServiceType.IsSubclassOf(typeof(DbContext)))
                    .Select(x => x.ServiceType);

                foreach (var dbContext in dbContexts)
                {
                    using var serviceScope = host.Services.CreateScope();
                    logger.LogInformation($"Migrating {dbContext.Name}");
                    var migrationTime = new Stopwatch();

                    var instanciatedDbContext = (DbContext)serviceScope.ServiceProvider.GetService(dbContext);

                    migrationTime.Start();
                    instanciatedDbContext.Database.Migrate();
                    migrationTime.Stop();

                    logger.LogInformation($"Migrating {dbContext.Name} finished in {migrationTime.ElapsedMilliseconds}");
                }

                logger.LogInformation("All migrations completed");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config
                            .AddJsonFile("appsettings.local.json", optional: true); //the file is gitignored, so each developer can have his own config
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services => {
                    services.AddSingleton(services);
                });
    }
}
