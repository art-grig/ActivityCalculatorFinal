using ActivityCalculator.Data;
using ActivityCalculator.Data.Entities;
using ActivityCalculator.Services.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ActivityCalculator.Services.Extensions;
using AutoMapper.QueryableExtensions;
using ActivityCalculator.Services.Exceptions;

namespace ActivityCalculator.Services.Impl
{
    public class DatasetService : IDatasetService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DatasetService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<MainMetricsVm> CalculateMainMetrics(long datasetId, int rollingRetentionDays = 7)
        {
            var lifeTimeChart = await _dbContext
                .Set<ActivityLog>()
                .Where(al => al.DatasetId == datasetId)
                .GroupBy(al => al.Lifetime)
                .Select(grp => new LifetimeChartItem
                {
                    LifetimeInDays = grp.Key,
                    NumberOfUsers = grp.Count()
                }).ToListAsync();


            decimal returned = await _dbContext.ActivityLogs
                .Where(al => al.DatasetId == datasetId
                    && al.Lifetime >= rollingRetentionDays)
                .CountAsync();

            decimal registered = await _dbContext.ActivityLogs
                .Where(al => al.DatasetId == datasetId
                    && al.RegistrationDate <= DateTime.UtcNow.AddDays(-rollingRetentionDays))
                .CountAsync();

            return new MainMetricsVm
            {
                LifetimeChart = lifeTimeChart,
                RollingRetention = returned / registered * 100
            };
        }

        public async Task<DatasetModel> CreateOrUpdate(DatasetModel model)
        {
            ActivityDataset dataSet = null;

            if (!model.Id.HasValue || model.Id == 0)
            {
                dataSet = _mapper.Map<ActivityDataset>(model);
                _dbContext.Set<ActivityDataset>().Add(dataSet);
            }
            else
            {
                dataSet = await _dbContext.Set<ActivityDataset>().GetByIdAsync(model.Id.Value) ?? throw new AppException($"Dataset with Id: {model.Id} doesn't exist");
                _mapper.Map(model, dataSet);
            }

            var newActivityLogs = _mapper.Map<List<ActivityLog>>(model.ActivityLogs);
            newActivityLogs.ForEach(al => al.Dataset = dataSet);
            _dbContext.Set<ActivityLog>().AddRange(newActivityLogs);
            model.DeletedIds?.Select(id => new ActivityLog { Id = id })
                .ToList().ForEach(e => _dbContext.Entry(e).State = EntityState.Deleted);

            await _dbContext.SaveChangesAsync();

            return await _dbContext.Set<ActivityDataset>()
                .Include(ad => ad.ActivityLogs)
                .ProjectTo<DatasetModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ad => ad.Id == dataSet.Id);
        }

        public async Task<DatasetModel> GetById(long datasetId)
        {
            return await _dbContext.Set<ActivityDataset>()
                .Include(ad => ad.ActivityLogs)
                .ProjectTo<DatasetModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ad => ad.Id == datasetId) ?? throw new AppException($"Dataset with Id: {datasetId} doesn't exist");
        }

        public async Task<IList<DatasetBaseModel>> List()
        {
            return await _dbContext.Set<ActivityDataset>().ProjectTo<DatasetBaseModel>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
