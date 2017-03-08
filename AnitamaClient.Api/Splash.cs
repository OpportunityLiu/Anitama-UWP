using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Splash\{ Id={Id} Title={Title} \}")]
    public class Splash
    {
        [JsonConstructor]
        private Splash([JsonProperty("url")]Image image, string origin)
        {
            image.Origin = origin;
            this.Image = image;
        }

        public Image Image { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("color")]
        public Color color { get; private set; }

        [JsonProperty("time")]
        public DateTimeOffset UpdatedTime { get; private set; }
    }
}
