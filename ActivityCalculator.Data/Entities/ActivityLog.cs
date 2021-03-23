using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Data.Entities
{
    public class ActivityLog : Entity
    {
        public long UserId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int Lifetime { get; set; }

        public long DatasetId { get; set; }
        public ActivityDataset Dataset { get; set; }
    }
}
