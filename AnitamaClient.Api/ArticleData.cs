using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"ArticleData\{ Id={Article.Id} Title={Article.Title} \}")]
    public class ArticleData
    {
        internal ArticleData() { }

        internal ArticleData(Article article)
        {
            this.Article = article;
        }

        private string articleId;

        [JsonProperty("article")]
        public Article Article
        {
            get => References.Articles[this.articleId];
            private set => this.articleId = References.Articles.Add(value);
        }

        [JsonProperty("relatedArticles")]
        public IList<Article> RelatedArticles { get; } = new ReferenceList<string, Article>();

        [JsonProperty("relatedComments")]
        public IList<Bangumi> RelatedComments { get; } = new ReferenceList<int, Bangumi>();

        private int topicId;

        [JsonProperty("topic")]
        public Topic Topic
        {
            get => References.Topics[this.topicId];
            private set => this.topicId = References.Topics.Add(value);
        }

        /// <summary>
        /// 是否为个人专栏
        /// </summary>
        [JsonProperty("verified")]
        public bool IsPersonalColumn { get; private set; }
    }
}
