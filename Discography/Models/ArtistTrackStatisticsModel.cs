
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Discography.Models
{
    public class ArtistTrackStatisticsModel
    {
        public ArtistTrackStatisticsModel()
        {
            Tracks = new List<TrackModel>();
        }

        public Guid ArtistId { get; set; }

        [DisplayName("Artist Name")]
        public string ArtistName { get; set; }

        [DisplayName("Track Count (Unique)")]
        public int TrackCount { get; set; }

        [DisplayName("Tracks with Lyrics")]
        public int TrackWithLyricCount { get; set; }

        [DisplayName("Word Count (Average)")]
        public decimal WordCountAverage { get; set; }
        
        [DisplayName("Word Count (Total)")]
        public int WordCount { get; set; }

        public List<TrackModel> Tracks { get; set; }
    }
}
