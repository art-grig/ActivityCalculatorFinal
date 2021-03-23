using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Data.Entities
{
    public class ActivityDataset : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
