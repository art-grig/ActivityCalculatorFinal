using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Data.Entities
{
    public abstract class Entity
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
