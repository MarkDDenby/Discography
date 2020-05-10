using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Artists
    {
        [JsonProperty("count")]
        public int TotalRowCount { get; set; }

        [JsonProperty("artists")]
        public List<ArtistSummary> Artist { get; set; }
    }
}
