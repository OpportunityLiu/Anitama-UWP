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
    public class Timeline : IncrementalLoadingCollection<FeedItem>
    {
        private static readonly string baseUri = "timeline";

        protected override IAsyncOperation<IEnumerable<FeedItem>> LoadPageAsync(int pageIndex)
        {
            var uri = $"{baseUri}?pageNo={pageIndex + 1}&pageSize=20";
            return Run<IEnumerable<FeedItem>>(async token =>
            {
                var req = Client.Current.GetAsync<DataPackage>(uri);
                token.Register(req.Cancel);
                var page = (await req).Page;
                this.RecordCount = page.TotalCount;
                this.PageCount = page.PageCount;
                var guides = page.Values.Where(v => v.EntryType == EntryType.Guide).ToArray();
                var i = this.Count - 20;
                if(i < 0)
                    i = 0;
                for(; i < this.Count; i++)
                {
                    var item = this[i];
                    if(item.EntryType == EntryType.Guide)
                    {
                        if(guides.Contains(item, comp))
                        {
                            this.RemoveAt(i);
                            i--;
                        }
                    }
                }
                return page.Values;
            });
        }

        private static FeedItemComparer comp = new FeedItemComparer();

        private class FeedItemComparer : IEqualityComparer<FeedItem>
        {
            public bool Equals(FeedItem x, FeedItem y)
            {
                if(x == null)
                {
                    if(y == null)
                        return true;
                    else
                        return false;
                }
                if(y == null)
                    return false;
                return x.ReleaseDate == y.ReleaseDate && x.Title == y.Title && x.EntryType == y.EntryType;
            }

            public int GetHashCode(FeedItem obj)
            {
                if(obj == null)
                    return 0;
                return obj.ReleaseDate.GetHashCode() ^ obj.Title.GetHashCode() ^ obj.EntryType.GetHashCode();
            }
        }

        private class DataPackage
        {
            [JsonProperty("page")]
            public ListPage<FeedItem> Page { get; private set; }
        }
    }
}
