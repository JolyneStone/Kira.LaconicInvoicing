using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Kira.LaconicInvoicing.Infrastructure.Json
{
    /// <summary>
    /// DateTime 转换类. Usage: [JsonConverter(typeof(DateTimeConverter))]
    /// </summary>
    public class DateTimeConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
}
