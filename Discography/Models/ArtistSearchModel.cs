using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Discography.Models
{
    public class ArtistSearchModel
    {
        public ArtistSearchModel()
        {
            Results = new List<ArtistModel>();
        }

        [Required]
        [DisplayName("Artist Name")]
        public string ArtistName { get; set; }
        public List<ArtistModel> Results { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public long ArtistCount { get; set; }
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < PageCount;
    }
}