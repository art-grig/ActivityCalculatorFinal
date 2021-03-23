using ActivityCalculator.Services.Exceptions;
using ActivityCalculator.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<GlobalExceptionHandler> log)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                LogException(log, ex);
                await WriteExceptionToResponse(ex, httpContext);
            }
        }

        private void LogException(ILogger log, Exception ex)
        {
            if (ex is AppException appException && appException.SystemMessage != null)
            {
                log.LogInformation(appException, appException.SystemMessage);
            }
            else
            {
                log.LogError(ex, ex.Message);
            }
        }

        private async Task WriteExceptionToResponse(Exception ex, HttpContext httpContext)
        {
            var responseVm = new BaseResponseVm(ex);

            httpContext.Response.ContentType = "application/json";

            var response = JsonConvert.SerializeObject(responseVm, _jsonSettings);

            await httpContext.Response.WriteAsync(response);
        }
    }
}
