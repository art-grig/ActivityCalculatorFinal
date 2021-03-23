using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityCalculator.Services.DI;
using ActivityCalculator.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using ActivityCalculator.Middlewares;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using ActivityCalculator.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddVersionedApiExplorer(c => {
                c.GroupNameFormat = "'v'VVV";
                c.SubstituteApiVersionInUrl = true;
                c.SubstitutionFormat = "VVV";
            });


            services.AddDbContext<AppDbContext>(c =>
                c.UseNpgsql(Configuration.GetConnectionString("App")));
            services.AddAppServices();

            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddApiVersion();

            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider,
            AutoMapper.IConfigurationProvider autoMapper, IApiVersionDescriptionProvider apiVersionDescriptionProvider, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseRouting();
            app.UseCors(options => options
                .SetIsOriginAllowed(x => _ = true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            autoMapper.AssertConfigurationIsValid();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                // build a swagger endpoint for each discovered API version
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions
                    .OrderByDescending(x => x.ApiVersion.MajorVersion.GetValueOrDefault())) {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
