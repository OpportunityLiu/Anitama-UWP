using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Article\{ Id={Id} Title={Title} \}")]
    public class Article : IPrimeryKey<string>
    {
        [JsonConstructor]
        public Article([JsonProperty("aid")]string id)
        {
            this.Id = id;
        }

        public IAsyncOperation<ArticleData> FetchDataAsync()
        {
            var data = new ArticleData(this);
            return Client.Current.GetAsync($"article/{this.Id}", data);
        }
        
        public string Id { get; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; internal set; }

        [JsonProperty("author")]
        public string Author { get; internal set; }

        [JsonProperty("origin")]
        public string Origin { get; internal set; }

        [JsonProperty("intro")]
        public string Introduction { get; internal set; }

        [JsonProperty("releaseDate")]
        public DateTimeOffset? ReleaseDate { get; internal set; }

        [JsonProperty("cover")]
        public Image Cover { get; internal set; }

        [JsonProperty("html")]
        public string Content { get; internal set; }

        [JsonProperty("channel")]
        public Channel Channel
        {
            get => References.Channels[channelId];
            internal set => this.channelId = References.Channels.Add(value);
        }

        private int channelId;

        [JsonProperty("tagList")]
        public ReferenceList<int, Tag> TagList { get; internal set; }

        string IPrimeryKey<string>.GetPrimeryKey() => this.Id;
    }
}
