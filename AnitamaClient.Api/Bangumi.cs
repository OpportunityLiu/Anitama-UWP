using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AnitamaClient.Api.Collections;
using System.Collections.ObjectModel;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Bangumi\{ Id={Id} Title={Title} \}")]
    public class Bangumi : IPrimeryKey<int>
    {
        [JsonConstructor]
        internal Bangumi([JsonProperty("bid")]int id)
        {
            this.Id = id;
            this.SeenEpisodes = new EpisodeList(this);
            this.RemindedEpisodes = new EpisodeList(this);
            this.Episodes = new EpisodeList(this);
        }

        public int Id { get; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("cover")]
        public Image Cover { get; internal set; }

        [JsonProperty("verticalCover")]
        public Image VerticalCover { get; internal set; }

        [JsonProperty("episode")]
        public string LatestEpisodeName { get; internal set; }

        [JsonProperty("playDate")]
        public DateTimeOffset? LatestEpisodePlayDate { get; internal set; }

        [JsonProperty("playSite")]
        public string PlaySiteName { get; internal set; }

        [JsonProperty("playUrl")]
        public Uri PlayUri { get; internal set; }

        [JsonProperty("playWeekday")]
        public string PlayWeekday { get; internal set; }

        [JsonProperty("playTime")]
        public string PlayTime { get; internal set; }

        [JsonProperty("originStation")]
        public string OriginStationName { get; internal set; }

        [JsonProperty("originWeekday")]
        public string OriginWeekday { get; internal set; }

        [JsonProperty("originTime")]
        public string OriginPlayTime { get; internal set; }


        [JsonProperty("mid")]
        public int MId { get; internal set; }

        [JsonProperty("episodeList", ItemConverterType = typeof(EpisodeConverter))]
        public IList<Episode> Episodes { get; }

        [JsonProperty("wantedList", ItemConverterType = typeof(EpisodeConverter))]
        public IList<Episode> RemindedEpisodes { get; }

        [JsonProperty("seenList", ItemConverterType = typeof(EpisodeConverter))]
        public IList<Episode> SeenEpisodes { get; }

        [JsonProperty("watch")]
        public bool Following { get; internal set; }

        int IPrimeryKey<int>.GetPrimeryKey() => this.Id;

        private class EpisodeList : ReferenceList<EpisodeKey, Episode>
        {
            private readonly Bangumi parent;

            public EpisodeList(Bangumi parent)
            {
                this.parent = parent;
            }

            protected override void InsertItem(int index, Episode item)
            {
                if(this.Keys.FindIndex(e => e.ENo == item.Number) > 0)
                    return;
                item.Bangumi = this.parent;
                item = References.Episodes[((IPrimeryKey<EpisodeKey>)item).GetPrimeryKey()] ?? item;
                base.InsertItem(index, item);
            }

            protected override void SetItem(int index, Episode item)
            {
                if(this.Keys.FindIndex(e => e.ENo == item.Number) > 0)
                    return;
                item.Bangumi = this.parent;
                item = References.Episodes[((IPrimeryKey<EpisodeKey>)item).GetPrimeryKey()] ?? item;
                base.SetItem(index, item);
            }
        }

        private class EpisodeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Episode);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var value = reader.Value.ToString();
                if(string.IsNullOrEmpty(value))
                    return null;
                return new Episode(value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
