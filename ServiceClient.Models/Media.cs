using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Media
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }
    }
}
