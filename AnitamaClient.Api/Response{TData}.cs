using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    public class Response<TData>
    {
        [JsonProperty("success")]
        public bool Success { get; private set; }

        [JsonProperty("info")]
        public string Info { get; private set; }

        [JsonProperty("status")]
        public int Status { get; private set; }

        [JsonProperty("data")]
        public TData Data { get; private set; }
    }
}
