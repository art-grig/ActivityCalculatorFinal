using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services.Models
{
    public class DatasetModel : DatasetBaseModel
    {
        public IList<ActivityLogModel> ActivityLogs { get; set; }
        public IList<long>? DeletedIds { get; set; }
    }
}
