using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Release
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("media")]
        public List<Media> Media { get; set; }
    }
}
