using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    public class DateTimeConverter : DateTimeConverterBase
    {
        private static readonly string[] formats = new string[]
        {
            "yyyyMMdd"
        };
        private static readonly System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.AssumeLocal | System.Globalization.DateTimeStyles.AllowWhiteSpaces | System.Globalization.DateTimeStyles.NoCurrentDateDefault;
        private static readonly IFormatProvider formatProvider = new System.Globalization.CultureInfo("zh-Hans-cn");

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime dt;
            var isTypeDt = objectType == typeof(DateTime) || objectType == typeof(DateTime?);
            switch(reader.TokenType)
            {
            case JsonToken.Integer:
                if(isTypeDt)
                    return DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(reader.Value)).LocalDateTime;
                else
                    return DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(reader.Value)).ToLocalTime();
            case JsonToken.String:
                var str = reader.Value.ToString();
                if(DateTime.TryParseExact(str, formats, formatProvider, style, out dt))
                    break;
                if(DateTime.TryParse(reader.Value.ToString(), formatProvider, style, out dt))
                    break;
                throw new FormatException("Unsupported date format.");
            default:
                throw new FormatException("Unsupported date format.");
            }
            if(isTypeDt)
                return dt;
            else
                return new DateTimeOffset(dt);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value is DateTimeOffset dto)
                writer.WriteValue(dto.ToString("O"));
            else if(value is DateTime dt)
                writer.WriteValue(dt.ToString("O"));
            else
                writer.WriteUndefined();
        }
    }

    public class TimeSpanConverter : JsonConverter
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
