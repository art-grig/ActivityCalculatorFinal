using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ActivityCalculator.Services.Impl;
using ActivityCalculator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ActivityCalculator.Services.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(c =>
                c.UseNpgsql(config.GetConnectionString("App")));

            services.AddAutoMapper(typeof(OrmMappingProfile));

            services.AddScoped<IDatasetService, DatasetService>();

            return services;
        }
    }
}
