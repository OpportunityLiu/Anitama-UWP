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
    public class Article : ObservableObject, IPrimeryKey<string>, IPopulatable
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

        private string content;
        [JsonProperty("html")]
        public string Content
        {
            get => this.content;
            internal set => this.Set(ref this.content, value);
        }

        [JsonProperty("channel")]
        public Channel Channel
        {
            get => References.Channels[channelId];
            internal set => this.channelId = References.Channels.Add(value);
        }

        private int channelId;

        [JsonProperty("tagList")]
        public ReferenceList<int, Tag> TagList { get; internal set; }

        public bool NeedPopulate => this.Content == null;

        string IPrimeryKey<string>.GetPrimeryKey() => this.Id;

        public IAsyncAction PopulateAsync()
        {
            return Run(async token =>
            {
                var t = FetchDataAsync();
                token.Register(t.Cancel);
                await t;
            });
        }
    }
}
