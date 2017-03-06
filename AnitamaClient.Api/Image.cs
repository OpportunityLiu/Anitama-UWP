using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Image{RawUri}")]
    [JsonConverter(typeof(ImageConverter))]
    public class Image
    {
        public string Origin { get; private set; }

        public Uri RawUri { get; private set; }

        /// <summary>
        /// 与 <see cref="RawUri"/> 相同。
        /// </summary>
        public Uri PreviewUri => new Uri(this.RawUri.OriginalString + "-preview");

        /// <summary>
        /// 压缩至 128x。
        /// </summary>
        public Uri ThumbUri => new Uri(this.RawUri.OriginalString + "-thumb");

        /// <summary>
        /// 裁剪至 240x180。
        /// </summary>
        public Uri CoverUri => new Uri(this.RawUri.OriginalString + "-cover");

        /// <summary>
        /// 裁剪至 1080x350。
        /// </summary>
        public Uri TopicUri => new Uri(this.RawUri.OriginalString + "-topic");

        private class ImageConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Image);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var ret = new Image();
                if(reader.TokenType== JsonToken.StartObject)
                {
                    while(reader.Read()&&reader.TokenType!= JsonToken.EndObject)
                    {
                        var pName = reader.Value.ToString();
                        var pValue = reader.ReadAsString();
                        switch(pName)
                        {
                        case "url":
                        case "thumb":
                            ret.RawUri = anlyzeUri(pValue);
                            break;
                        case "origin":
                            ret.Origin = pValue;
                            break;
                        }
                    }
                }
                else
                {
                    ret.RawUri = anlyzeUri(reader.Value.ToString());
                }
                return ret;
            }

            private static readonly Regex analyzeUriReg = new Regex(@"^(.+?)(-[-\w]+)?$", RegexOptions.Compiled | RegexOptions.Singleline);

            private Uri anlyzeUri(string pValue)
            {
                var match = analyzeUriReg.Match(pValue);
                if(!match.Success)
                    throw new FormatException("Wrong image uri format.");
                return new Uri(match.Groups[1].Value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
