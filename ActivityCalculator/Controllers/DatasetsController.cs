using ActivityCalculator.Services;
using ActivityCalculator.Services.Models;
using ActivityCalculator.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.Controllers
{
    public class DatasetsController : BaseController
    {
        private readonly IDatasetService _datasetService;

        public DatasetsController(IDatasetService datasetService)
        {
            _datasetService = datasetService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseVm<DatasetModel>>> CreateOrUpdate(DatasetModel model)
        {
            return ApiResponse(await _datasetService.CreateOrUpdate(model));
        }

        [HttpGet]
        public async Task<ActionResult<ResponseVm<IList<DatasetBaseModel>>>> List()
        {
            return ApiResponse(await _datasetService.List());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseVm<DatasetModel>>> Get(long id)
        {
            return ApiResponse(await _datasetService.GetById(id));
        }

        [HttpGet("calculate")]
        public async Task<ActionResult<ResponseVm<MainMetricsVm>>> Calculate(long datasetId, int rollingRetentionDays = 7)
        {
            return ApiResponse(await _datasetService.CalculateMainMetrics(datasetId, rollingRetentionDays));
        }
    }
}
