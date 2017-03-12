using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnitamaClient.Api.Collections;
using Windows.Foundation;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace AnitamaClient.Api
{
    [System.Diagnostics.DebuggerDisplay(@"Guide\{ Date={Date:d} Count={RemindedEpisodeList.Count + EpisodeList.Count} \}")]
    public class Guide : IPopulatable
    {
        internal Guide(DateTimeOffset date)
        {
            this.Date = date.Date;
        }

        public DateTime Date { get; }

        [JsonProperty("reminder")]
        public IList<Episode> RemindedEpisodeList { get; } = new ReferenceList<EpisodeKey, Episode>();
        [JsonProperty("list")]
        public IList<Episode> EpisodeList { get; } = new ReferenceList<EpisodeKey, Episode>();
        [JsonProperty("seen")]
        public IList<int> SeenEpisodeBIdList { get; } = new List<int>();

        public bool NeedPopulate { get; private set; } = true;

        public IAsyncAction PopulateAsync()
        {
            return Run(async token =>
            {
                if(!this.NeedPopulate)
                {
                    this.RemindedEpisodeList.Clear();
                    this.EpisodeList.Clear();
                    this.SeenEpisodeBIdList.Clear();
                }
                var t = Client.Current.GetAsync($"guide/{this.Date:yyyyMMdd}", this);
                token.Register(t.Cancel);
                await t;
                foreach(var item in this.RemindedEpisodeList)
                {
                    item.Bangumi.RemindedEpisodes.Add(item);
                    item.Bangumi.Episodes.Add(item);
                }
                foreach(var item in this.EpisodeList)
                {
                    item.Bangumi.Episodes.Add(item);
                }
                foreach(var bid in this.SeenEpisodeBIdList)
                {
                    var bangumi = References.Bangumis[bid];
                    bangumi.SeenEpisodes.Add(this.RemindedEpisodeList.Concat(this.EpisodeList).First(e => e.Bangumi.Id == bid));
                }
                this.NeedPopulate = false;
            });
        }
    }
}
