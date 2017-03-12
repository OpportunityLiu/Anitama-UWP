﻿using Newtonsoft.Json;
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
                g.PopulateAsync();
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
    //    {
    //    "entryType": "article",
    //    "aid": "65df2bda0930800d",
    //    "title": "这难道就是恋爱？！",
    //    "subtitle": "Anitama新闻汇总",
    //    "author": "LIAR",
    //    "origin": "Anitama",
    //    "intro": "2017年3月4日新闻汇总",
    //    "releaseDate": "2017-03-05T06:00:07+08:00",
    //    "cover": {
    //        "url": "http://cdn.animetamashi.cn/65df2bda0930800d/img/0151c1-preview",
    //        "origin": "《神击的巴哈姆特 VIRGIN SOUL》",
    //        "cover": "http://cdn.animetamashi.cn/65df2bda0930800d/img/0151c1-cover"
    //    }
    //}
    //{
    //    "entryType": "guide",
    //    "title": "03月05日节目预告",
    //    "subtitle": "Anitama节目预告",
    //    "cover": "http://img.animetamashi.cn/guide/be8556-cover",
    //    "releaseDate": "2017-03-05T05:00:00+08:00"
    //}
}
