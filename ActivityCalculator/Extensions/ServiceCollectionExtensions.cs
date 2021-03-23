using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiVersion(this IServiceCollection services,
            ApiVersion defaultApiVersion = null, bool versioningByNamespace = false)
        {
            services.AddApiVersioning(options => {
                if (defaultApiVersion != null)
                {
                    options.DefaultApiVersion = defaultApiVersion;
                }

                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.AssumeDefaultVersionWhenUnspecified = true;

                if (versioningByNamespace)
                {
                    options.Conventions.Add(new VersionByNamespaceConvention());
                }
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c => {

                var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

                if (provider?.ApiVersionDescriptions != null)
                {
                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        var info = new OpenApiInfo
                        {
                            Title = $"ActivityCalculator API {description.ApiVersion.ToString()}",
                            Version = description.ApiVersion.ToString()
                        };

                        if (description.IsDeprecated)
                        {
                            info.Description += "DEPRECATED";
                        }

                        c.SwaggerDoc(description.GroupName, info);
                    }
                }
            });
        }
    }
}
