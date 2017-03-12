using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"\{Success={Success} Info={Info} Status={Status}\}")]
    public class Response<TData>
    {
        public Response() { }

        public Response(TData data)
        {
            this.Data = data;
        }

        [JsonProperty("success")]
        public bool Success { get; private set; }

        [JsonProperty("info")]
        public string Info { get; private set; }

        [JsonProperty("status")]
        public int Status { get; private set; }

        [JsonProperty("data", ObjectCreationHandling = ObjectCreationHandling.Auto)]
        public TData Data { get; private set; }
    }
}
