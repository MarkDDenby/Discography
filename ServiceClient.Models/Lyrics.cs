using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Lyrics
    {
        [JsonProperty("lyrics")]
        public string Lyric { get; set; }
    }
}
