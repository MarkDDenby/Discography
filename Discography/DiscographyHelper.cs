using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discography.Contracts;
using Discography.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServiceClient.Contracts;
using ServiceClient.Models;

namespace Discography
{
    // automapper should be used to map between the service models and the returned view models

    public class DiscographyHelper : IDiscographyHelper
    {
        private readonly ILogger _logger;
        private readonly IDiscographyServiceClient _serviceClient;
        private readonly IWordCounter _wordCounter;
        private readonly IHubContext<NotificationHub> _notificationHub;

        private const int RowLimit = 25;

        public DiscographyHelper(ILogger<DiscographyHelper> logger, IDiscographyServiceClient serviceClient, IWordCounter wordCounter, IHubContext<NotificationHub> notificationHub)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
            _wordCounter = wordCounter ?? throw new ArgumentNullException(nameof(wordCounter));
            _notificationHub = notificationHub ?? throw new ArgumentNullException(nameof(notificationHub));
        }

        public async Task<ArtistTrackStatisticsModel> GetArtistStatistics(Guid artistId, CancellationToken cancellationToken)
        {
            var artist = await GetArtist(artistId);
            var releases = await GetReleases(artistId, artist.Name, cancellationToken);
            var uniqueTracks = releases.SelectMany(m => m.Media).SelectMany(t => t.Tracks).Distinct().ToList();

            uniqueTracks = await GetLyrics(cancellationToken, uniqueTracks, artist);

            return PopulateViewModel(artistId, artist, uniqueTracks);
        }

        private ArtistTrackStatisticsModel PopulateViewModel(Guid artistId, Artist artist, List<Track> tracks)
        {
            var model = new ArtistTrackStatisticsModel
            {
                ArtistName = artist.Name,
                ArtistId = artistId,
                TrackCount = tracks.Count,
                WordCount = tracks.Sum(o => o.WordCount),
                TrackWithLyricCount = tracks.Count(o => o.WordCount > 0)
            };

            if (model.WordCount > 0)
            {
                model.WordCountAverage = Convert.ToDecimal(model.WordCount) / Convert.ToDecimal(model.TrackWithLyricCount);
            }

            foreach (var track in tracks)
            {
                model.Tracks.Add(new TrackModel() { Title = track.Title });
            }

            return model;
        }

        private async Task<List<Track>> GetLyrics(CancellationToken cancellationToken, List<Track> tracks, Artist artist)
        {
            var processCount = 0;

            foreach (var track in tracks)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var lyrics = await GetLyric(artist.Name, track.Title);
                if (lyrics != null)
                {
                    track.Lyrics = lyrics.Lyric;
                    track.WordCount = await CountWords(track.Lyrics);
                }

                processCount++;

                if (_notificationHub.Clients != null)
                {
                    var percent = (100 * processCount) / tracks.Count;
                    await _notificationHub.Clients.All.SendAsync("displayFeedback",$"{percent}% Done - Retrieving and counting lyrics for track '{track.Title}'",cancellationToken);
                }
            }

            return tracks;
        }

        private async Task<List<Release>> GetReleases(Guid artistId, string artistName, CancellationToken cancellationToken)
        {
            var rowsRetrieved = 0;
            var totalRowCount = 1;
            var releases = new List<Release>();

            // page through the result set.
            while (rowsRetrieved < totalRowCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await GetReleases(artistId, rowsRetrieved);
                if (result.Release.Count == 0)
                {
                    break;
                }

                totalRowCount = result.TotalRowCount;
                releases.AddRange(result.Release);
                rowsRetrieved = rowsRetrieved + result.Release.Count;

                if (_notificationHub.Clients != null)
                {
                    var percent = (100 * rowsRetrieved) / totalRowCount;
                    await _notificationHub.Clients.All.SendAsync("displayFeedback",$"{percent}% Done - Retrieving releases for artist '{artistName}'", cancellationToken);
                }
            }

            return releases;
        }

        public async Task<ArtistSearchModel> ArtistSearch(string artistName, int currentPage)
        {
            var offset = (currentPage - 1) * RowLimit;
            var artistsQuery = await GetArtists(artistName, offset);

            var model = new ArtistSearchModel
            {
                ArtistName = artistName,
                ArtistCount = artistsQuery.TotalRowCount,
                CurrentPage = currentPage,
                PageCount = (int) Math.Ceiling(Convert.ToDecimal(artistsQuery.TotalRowCount) / Convert.ToDecimal(RowLimit))
            };

            foreach (var artist in artistsQuery.Artist)
            {
                model.Results.Add(new ArtistModel()
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Country = artist.Country,
                    Type = artist.Type,
                    Disambiguation = artist.Disambiguation
                });
            }

            return model;
        }

        private async Task<Artist> GetArtist(Guid artistId)
        {
            _logger.LogInformation($"DiscographyHelper GetArtist({artistId})");

            return await _serviceClient.GetArtist(artistId);
        }

        private async Task<Artists> GetArtists(string name, int offset)
        {
            _logger.LogInformation($"DiscographyHelper GetArtists({name}, {RowLimit}, {offset})");

            return await _serviceClient.GetArtists(name, RowLimit, offset);
        }

        private async Task<Releases> GetReleases(Guid artistId, int offset)
        {
            _logger.LogInformation($"DiscographyHelper GetReleases({artistId}, {RowLimit}, {offset})");

            return await _serviceClient.GetReleases(artistId, RowLimit, offset);
        }

        private async Task<Lyrics> GetLyric(string artistName, string title)
        {
            return await _serviceClient.GetLyrics(artistName,title);
        }

        private async Task<int> CountWords(string words)
        {
            return await Task.Run(() => _wordCounter.Count(words));
        }
    }
}