using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace AnitamaClient.Api.JsonConverters
{
    internal class TimeSpanConverter : JsonConverter
    {
        private static readonly Regex thirtyHourParser = new Regex(@"^\s*(\d\d)\s*:\s*(\d\d)\s*$", RegexOptions.Singleline | RegexOptions.Compiled);

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var v = reader.Value.ToString();
            var mat = thirtyHourParser.Match(v);
            if(mat.Success)
                return new TimeSpan(int.Parse(mat.Groups[1].Value), int.Parse(mat.Groups[2].Value), 0);
            throw new FormatException("Unsupported time format.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (TimeSpan)value;
            writer.WriteValue($"{v.Days * 24 + v.Hours}:{v.Minutes}");
        }
    }
}
