using System;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    public class Track : IEquatable<Track>
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        public int WordCount { get; set; }

        public string Lyrics { get; set; }

        public bool Equals(Track other)
        {
            if (other == null)
            {
                return false;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return (Title.Equals(other.Title, StringComparison.CurrentCultureIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Track);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (Title != null)
            {
                hash ^= Title.GetHashCode();
            }

            return hash;
        }

    }
}
