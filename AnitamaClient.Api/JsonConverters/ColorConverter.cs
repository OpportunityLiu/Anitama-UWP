using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AnitamaClient.Api.JsonConverters
{
    internal class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var colorStr = reader.Value.ToString();
            if(colorStr.StartsWith("rgba("))
            {
                var splits = colorStr.Split(new[] { '(', ')', ',' });
                return Color.FromArgb((byte)((1 - double.Parse(splits[4])) * 255), byte.Parse(splits[1]), byte.Parse(splits[2]), byte.Parse(splits[3]));
            }
            if(colorStr.StartsWith("rgb("))
            {
                var splits = colorStr.Split(new[] { '(', ')', ',' });
                return Color.FromArgb(255, byte.Parse(splits[1]), byte.Parse(splits[2]), byte.Parse(splits[3]));
            }
            if(colorStr.StartsWith("#"))
            {
                var r = "ff";
                var g = "ff";
                var b = "ff";
                if(colorStr.Length == 7)
                {
                    r = colorStr.Substring(1, 2);
                    g = colorStr.Substring(3, 2);
                    b = colorStr.Substring(5, 2);
                }
                else if(colorStr.Length == 4)
                {
                    r = new string(colorStr[1], 2);
                    g = new string(colorStr[2], 2);
                    b = new string(colorStr[3], 2);
                }
                return Color.FromArgb(255, byte.Parse(r, System.Globalization.NumberStyles.HexNumber), byte.Parse(g, System.Globalization.NumberStyles.HexNumber), byte.Parse(b, System.Globalization.NumberStyles.HexNumber));
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (Color)value;
            writer.WriteValue($"rgba({v.R},{v.G},{v.B},{1 - (v.A / 255.0)})");
        }
    }
}
