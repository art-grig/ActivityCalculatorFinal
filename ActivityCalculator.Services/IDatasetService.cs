using ActivityCalculator.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ActivityCalculator.Services
{
    public interface IDatasetService
    {
        Task<DatasetModel> GetById(long datasetId);
        Task<DatasetModel> CreateOrUpdate(DatasetModel model);
        Task<IList<DatasetBaseModel>> List();
        Task<MainMetricsVm> CalculateMainMetrics(long datasetId, int rollingRetentionDays = 7);
    }
}
