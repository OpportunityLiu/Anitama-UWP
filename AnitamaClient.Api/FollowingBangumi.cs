using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"FollowingBangumi\{ Id={Id} Bangumi={Bangumi.Title} \}")]
    public class FollowingBangumi
    {
        [JsonProperty("bid")]
        public int bid { get; set; }
        [JsonProperty("cover")]
        public string cover { get; set; }
        [JsonProperty("verticalCover")]
        public string verticalCover { get; set; }
        [JsonProperty("playUrl")]
        public string playUrl { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }

    }
}
