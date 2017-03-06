using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Channel\{ Id={Id} Name={Name} \}")]
    public class Channel : IPrimeryKey<int>
    {
        [JsonProperty("channelId")]
        public int Id { get; set; }

        [JsonProperty("channelName")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("img")]
        public Image Image { get; set; }

        int IPrimeryKey<int>.GetPrimeryKey() => this.Id;
    }
}
