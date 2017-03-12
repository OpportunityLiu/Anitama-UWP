using AnitamaClient.Api.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Page\{ {StartIndex + 1}-{StartIndex + (Values?.Count ?? 0)} of {TotalCount} \}")]
    public abstract class Page<TValue>
    {
        [JsonProperty("pageNo")]
        public int PageIndex { get; private set; }

        [JsonProperty("pageSize")]
        public int Capacity { get; private set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; private set; }

        [JsonIgnore]
        public int PageCount 
            => this.TotalCount / this.Capacity + (this.TotalCount % this.Capacity == 0 ? 0 : 1);

        [JsonProperty("start")]
        public int StartIndex { get; private set; }

        [JsonProperty("list")]
        public abstract IList<TValue> Values { get; }
    }
    
    public class ListPage<TValue> : Page<TValue>
    {
        public override IList<TValue> Values { get; } = new List<TValue>();
    }

    public class RefListPage<TKey, TValue> : Page<TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        public override IList<TValue> Values { get; } = new ReferenceList<TKey, TValue>();
    }
}
