using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"FeedItem\{ EntryType={EntryType} Link={Link} \}")]
    public class FeedItem
    {
        [JsonConstructor]
        public FeedItem(EntryType entryType,
            string title,
            string subtitle,
            Image cover,
            DateTimeOffset releaseDate,
            string aid,
            string author,
            string origin,
            string intro)
        {
            this.EntryType = entryType;
            this.ReleaseDate = releaseDate;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Cover = cover;
            switch(entryType)
            {
            case EntryType.Article:
                var a = Collections.References.Articles[aid];
                if(a == null)
                {
                    a = new Article(aid);
                    Collections.References.Articles.Add(a);
                }
                a.Title = title;
                a.Subtitle = subtitle;
                a.Cover = cover;
                a.ReleaseDate = releaseDate;
                a.Author = author;
                a.Origin = origin;
                a.Introduction = intro;
                this.Item = a;
                break;
            case EntryType.Guide:
                var g = new Guide(releaseDate);
                this.Item = g;
                break;
            }
        }

        public EntryType EntryType { get; }

        public string Title { get; }

        public string Subtitle { get; }

        public Image Cover { get; }

        public DateTimeOffset ReleaseDate { get; }

        public object Item { get; }
    }

    public enum EntryType
    {
        Article,
        Guide
    }
}
