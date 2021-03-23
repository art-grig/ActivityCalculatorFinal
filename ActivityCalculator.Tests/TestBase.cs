using ActivityCalculator.Data;
using ActivityCalculator.Services.DI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ActivityCalculator.Tests
{
    public class TestBase : IAsyncLifetime
    {
        protected IServiceProvider _serviceProvider;
        protected AppDbContext _dbContext;
        protected ITestOutputHelper _output;
        protected IMapper _mapper;
        protected SqliteConnection _keepAliveConnection;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public async Task DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _keepAliveConnection.CloseAsync();
        }

        protected virtual Task CreateDatabase()
        {
            return _dbContext.Database.EnsureCreatedAsync();
        }

        public async Task InitializeAsync()
        {
            _keepAliveConnection = new SqliteConnection("DataSource=:memory:");
            await _keepAliveConnection.OpenAsync();

            var services = new ServiceCollection()
                .AddDbContext<AppDbContext>(c => c.UseSqlite(_keepAliveConnection))
                .AddAppServices();

            var partManager = new ApplicationPartManager();
            partManager.FeatureProviders.Add(new ControllerFeatureProvider());
            partManager.ApplicationParts.Add(new AssemblyPart(Assembly.Load("ActivityCalculator")));
            var controllersFeature = new ControllerFeature();
            partManager.PopulateFeature(controllersFeature);
            foreach (var controllerType in controllersFeature.Controllers.Select(t => t.AsType()))
            {
                services.TryAddTransient(controllerType);
            }

            _serviceProvider = services.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();

            await _dbContext.Database.EnsureDeletedAsync();
            await CreateDatabase();
            await Seed();
        }

        protected virtual async Task Seed()
        {

        }
    }
}
