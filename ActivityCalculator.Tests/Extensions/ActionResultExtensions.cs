using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ActivityCalculator.Tests.Extensions
{
    public static class ActionResultExtensions
    {
        public static TResponse ExtractResponse<TResponse>(this ActionResult<TResponse> response)
        {
            Assert.NotNull(response);
            var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
            var typedResponse = Assert.IsAssignableFrom<TResponse>(okObjectResult.Value);
            Assert.NotNull(typedResponse);
            return typedResponse;
        }
    }
}
