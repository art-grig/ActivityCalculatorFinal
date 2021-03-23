using ActivityCalculator.Controllers;
using ActivityCalculator.Data.Entities;
using ActivityCalculator.Services.Models;
using ActivityCalculator.Tests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ActivityCalculator.Tests
{
    public class DatasetsControllerTests : TestBase
    {
        public DatasetsControllerTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task DataSetCreationAndMetricsCalculation_Success()
        {
            var datasetsController = _serviceProvider.GetRequiredService<DatasetsController>();

            var dataSetModel = new DatasetModel
            {
                Description = "test",
                Name = "test",
                ActivityLogs = Enumerable.Range(1, 10).Select(i => new ActivityLogModel
                {
                    UserId = i,
                    RegistrationDate = new DateTime(2021, 1, i),
                    LastActivityDate = new DateTime(2021, 2, i)
                }).ToList()
            };
            var creationResp = await datasetsController.CreateOrUpdate(dataSetModel);
            var creationRes = creationResp.ExtractResponse();

            var calcResp = await datasetsController.Calculate(creationRes.Data.Id.Value);
            var calcRes = calcResp.ExtractResponse();

            calcRes.Success.Should().BeTrue();
            calcRes.Data.LifetimeChart.Should().BeEquivalentTo(new List<object> { new { LifetimeInDays = 31, NumberOfUsers = 10 } });
            calcRes.Data.RollingRetention.Should().Be(100);
        }
    }
}
