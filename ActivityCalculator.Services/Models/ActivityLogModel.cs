using ActivityCalculator.Services.Attributes;
using ActivityCalculator.Services.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ActivityCalculator.Services.Models
{
    public class ActivityLogModel
    {
        public long? Id { get; set; }
        public long UserId { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime RegistrationDate { get; set; }

        [GreaterThan(nameof(RegistrationDate), true)]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime LastActivityDate { get; set; }
    }
}
