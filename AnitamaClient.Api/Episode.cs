using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{

    [System.Diagnostics.DebuggerDisplay(@"Episode\{ Bangumi={Bangumi?.Title} Name={Name} \}")]
    public class Episode
    {
        [JsonConstructor]
        internal Episode([JsonProperty("bid")]int bangumiId,
            [JsonProperty("title")]string title,
            [JsonProperty("cover")]Image cover,
            [JsonProperty("verticalCover")]Image verticalCover,
            [JsonProperty("playSite")]string playSite,
            [JsonProperty("originStation")]string originStation,
            [JsonProperty("playUrl")]Uri playUrl,
            [JsonProperty("playTime")]TimeSpan? playTime,
            [JsonProperty("originTime")]TimeSpan? originTime)
        {
            this.bangumiId = bangumiId;
            if(this.Bangumi == null)
                References.Bangumis.Add(new Bangumi(bangumiId));
            var b = this.Bangumi;
            if(title != null)
                b.Title = title;
            if(cover != null)
                b.Cover = cover;
            if(verticalCover != null)
                b.VerticalCover = verticalCover;
            if(playTime is TimeSpan pt)
                b.PlayTime = pt;
            if(originTime is TimeSpan ot)
                b.OriginPlayTime = ot;
            if(playSite != null)
                b.PlaySiteName = playSite;
            if(originStation != null)
                b.OriginStationName = originStation;
            if(playUrl != null)
                b.PlayUri = playUrl;
        }

        private int bangumiId;

        public Bangumi Bangumi => References.Bangumis[this.bangumiId];

        [JsonProperty("episode")]
        public string Name { get; private set; }

        [JsonProperty("episodeNo")]
        public string Number { get; private set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset? PlayDateTime { get; private set; }

        [JsonProperty("originTimestamp")]
        public DateTimeOffset? OriginPlayDateTime { get; private set; }

        [JsonProperty("isManaged")]
        public bool IsManaged { get; private set; }
    }
}
