using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services.Models
{
    public class MainMetricsVm
    {
        public IList<LifetimeChartItem> LifetimeChart { get; set; }
        public decimal RollingRetention { get; set; }
    }
}
