using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Releases
    {
        [JsonProperty("release-count")]
        public int TotalRowCount { get; set; }

        [JsonProperty("releases")]
        public List<Release> Release { get; set; }
    }
}
