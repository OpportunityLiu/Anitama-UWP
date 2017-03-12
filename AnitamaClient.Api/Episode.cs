using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{

    [System.Diagnostics.DebuggerDisplay(@"Episode\{ Bangumi={Bangumi?.Title} Name={Name} \}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Episode : IPrimeryKey<EpisodeKey>
    {
        [JsonConstructor]
        internal Episode([JsonProperty("bid")]int bangumiId,
            [JsonProperty("title")]string title,
            [JsonProperty("cover")]Image cover,
            [JsonProperty("verticalCover")]Image verticalCover,
            [JsonProperty("playSite")]string playSite,
            [JsonProperty("originStation")]string originStation,
            [JsonProperty("playUrl")]Uri playUrl)
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
            if(playSite != null)
                b.PlaySiteName = playSite;
            if(originStation != null)
                b.OriginStationName = originStation;
            if(playUrl != null)
                b.PlayUri = playUrl;
        }

        internal Episode(string eNo)
        {
            this.Number = eNo;
        }

        private int bangumiId;

        public Bangumi Bangumi
        {
            get => References.Bangumis[this.bangumiId];
            set => this.bangumiId = References.Bangumis.Add(value);
        }

        [JsonProperty("episode")]
        public string Name { get; private set; }

        [JsonProperty("episodeNo")]
        public string Number { get; private set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset? PlayDateTime { get; private set; }

        [JsonProperty("originTimestamp")]
        public DateTimeOffset? OriginPlayDateTime { get; private set; }

        [JsonProperty("isManaged")]
        public bool CanMarkAsSeen { get; private set; }

        EpisodeKey IPrimeryKey<EpisodeKey>.GetPrimeryKey()
        {
            return new EpisodeKey(this.bangumiId, this.Number);
        }
    }

    public struct EpisodeKey : IEquatable<EpisodeKey>
    {
        public EpisodeKey(int bId, string eNo)
        {
            this.BId = bId;
            this.ENo = eNo;
        }

        public readonly int BId;
        public readonly string ENo;

        public bool Equals(EpisodeKey other)
        {
            return this.BId == other.BId && this.ENo == other.ENo;
        }
        
        public override bool Equals(object obj)
        {
            if(obj is Episode ep)
            {
                return Equals(ep);
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return this.BId ^ this.ENo.GetHashCode();
        }
    }
}
