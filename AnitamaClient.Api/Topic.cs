using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Topic\{Id = {Id} Name = {Name}\}")]
    public class Topic : IPrimeryKey<int>
    {
        [JsonProperty("tid")]
        public int Id { get; private set; }

        [JsonProperty("topicName")]
        public string Name { get; private set; }

        [JsonProperty("topicCover")]
        public Image Cover { get; private set; }

        [JsonProperty("lastUpdateDate")]
        public DateTimeOffset? LastUpdateDate { get; private set; }

        private string lastUpdateArticleId;

        [JsonProperty("lastUpdateArticle")]
        public Article LastUpdateArticle
        {
            get => References.Articles[this.lastUpdateArticleId];
            private set => this.lastUpdateArticleId = References.Articles.Add(value);
        }

        public int GetPrimeryKey() => this.Id;
    }
}
