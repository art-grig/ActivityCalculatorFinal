using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityCalculator.Services.Converters
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string _format = "dd'.'MM'.'yyyy";

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), _format, System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }

}
