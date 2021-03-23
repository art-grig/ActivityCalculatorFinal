using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services.Models
{
    public class DatasetBaseModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
