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
    public class Page<TKey, TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        [JsonProperty("pageNo")]
        public int PageIndex { get; private set; }

        [JsonProperty("pageSize")]
        public int Capacity { get; private set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; private set; }

        [JsonProperty("list")]
        public ReferenceList<TKey, TValue> Values { get; private set; }

        [JsonProperty("start")]
        public int StartIndex { get; private set; }
    }
}
