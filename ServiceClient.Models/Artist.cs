using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Artist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}