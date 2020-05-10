using System;

namespace ServiceClient.Models
{
    public class ArtistSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Disambiguation { get; set; }
    }
}
