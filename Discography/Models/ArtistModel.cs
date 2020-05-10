using System;

namespace Discography.Models
{
    public class ArtistModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Disambiguation { get; set; }
    }
}
