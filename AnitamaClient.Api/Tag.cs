using System;
using Newtonsoft.Json;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Tag\{ Id={Id} Name={Name} \}")]
    public class Tag : IPrimeryKey<int>
    {
        [JsonProperty("tagId")]
        public int Id { get; private set; }

        [JsonProperty("tagName")]
        public string Name { get; private set; }

        int IPrimeryKey<int>.GetPrimeryKey() => this.Id;
    }
}