using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discography.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using ServiceClient.Contracts;
using Microsoft.AspNetCore.SignalR;
using ServiceClient.Models;

namespace Discography.Tests
{
    [TestClass]
    public class DiscographyHelperTests
    {
        private Mock<ILogger<DiscographyHelper>> _mockLogger;
        private Mock<IDiscographyServiceClient> _mockServiceClient;
        private Mock<IWordCounter> _mockWordCounter;
        private Mock<IHubContext<NotificationHub>> _mockNotificationHub;
        
        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<DiscographyHelper>>();
            _mockServiceClient = new Mock<IDiscographyServiceClient>();
            _mockWordCounter = new Mock<IWordCounter>();
            _mockNotificationHub = new Mock<IHubContext<NotificationHub>>();
        }

        [TestMethod]
        public void GetArtistStatisticsReturnsCorrectViewModel()
        {
            var expectedUniqueTrackCount = 5;
            var expectedTotalWordCount = 20;
            var expectedWordCountAverage = 4;
            var expectedArtistName = "Artist Name";

            var cancellationToken = new CancellationToken();

            var artist = new Artist() { Name = expectedArtistName };

            var media1Tracks = new List<Track>
            {
                new Track() { Title = "Test Song 1" },
                new Track() { Title = "Test Song 2" },
                new Track() { Title = "Test Song 3" }
            };

            var media2Tracks = new List<Track>
            {
                new Track() {Title = "Test Song 1"},
                new Track() {Title = "Test Song 2"},
                new Track() {Title = "Test Song 3"},
                new Track() {Title = "Test Song 4"},
                new Track() {Title = "Test Song 5"}
            };

            var media1 = new Media
            {
                Tracks = new List<Track>(media1Tracks)
            };

            var media2 = new Media
            {
                Tracks = new List<Track>(media2Tracks)
            };

            var release = new Release
            {
                Status = "Official",
                Media = new List<Media>() { media1, media2 }
            };

            var releases = new Releases
            {
                Release = new List<Release>() { release },
                TotalRowCount = 1
            };

            var lyrics = new Lyrics()
            {
                Lyric = "These Are Test Lyrics"
            };

            _mockServiceClient
                .Setup(o => o.GetArtist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(artist));

            _mockServiceClient
                .Setup(o => o.GetReleases(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(releases));

            _mockServiceClient
                .Setup(o => o.GetLyrics(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(lyrics));

            _mockWordCounter
                .Setup(o => o.Count(It.IsAny<string>()))
                .Returns(4);

            var sut = new DiscographyHelper(_mockLogger.Object, _mockServiceClient.Object, _mockWordCounter.Object, _mockNotificationHub.Object);

            var model = sut.GetArtistStatistics(Guid.NewGuid(), cancellationToken);

            Assert.IsNotNull(model);
            Assert.AreEqual(expectedUniqueTrackCount, model.Result.TrackCount);
            Assert.AreEqual(expectedUniqueTrackCount, model.Result.Tracks.Count);
            Assert.AreEqual(expectedUniqueTrackCount, model.Result.TrackWithLyricCount);
            Assert.AreEqual(expectedTotalWordCount, model.Result.WordCount);
            Assert.AreEqual(expectedWordCountAverage, model.Result.WordCountAverage);
            Assert.AreEqual(expectedArtistName, model.Result.ArtistName);
        }

        [TestMethod]
        public void ArtistSearchReturnsCorrectViewModelForFirstPage()
        {
            var page = 1;
            var totalArtistCount = 4;
            var totalPageCount = 2;
            var artists = new Artists
            {
                Artist = new List<ArtistSummary>(),
                TotalRowCount = 50
            };

            artists.Artist.Add(new ArtistSummary() { Name = "Artist One", Country = "UK", Disambiguation = "Description for artist one", Id = Guid.NewGuid(), Type = "Group"});
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Two", Country = "UK", Disambiguation = "Description for artist two", Id = Guid.NewGuid(), Type = "Singer" });
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Three", Country = "US", Disambiguation = "Description for artist three", Id = Guid.NewGuid(), Type = "Group" });
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Four", Country = "JPN", Disambiguation = "Description for artist four", Id = Guid.NewGuid(), Type = "Composer" });

            _mockServiceClient
                .Setup(o => o.GetArtists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(artists));

            var sut = new DiscographyHelper(_mockLogger.Object, _mockServiceClient.Object, _mockWordCounter.Object, _mockNotificationHub.Object);
            var model = sut.ArtistSearch("Artist", page);

            Assert.AreEqual(totalArtistCount, model.Result.Results.Count);
            Assert.AreEqual(totalPageCount, model.Result.PageCount);
            Assert.AreEqual(page, model.Result.CurrentPage);
            Assert.IsFalse(model.Result.ShowPrevious);
            Assert.IsTrue(model.Result.ShowNext);
        }

        [TestMethod]
        public void ArtistSearchReturnsCorrectViewModelForSecondPage()
        {
            var page = 2;
            var totalArtistCount = 4;
            var totalPageCount = 2;
            var artists = new Artists
            {
                Artist = new List<ArtistSummary>(),
                TotalRowCount = 50
            };

            artists.Artist.Add(new ArtistSummary() { Name = "Artist One", Country = "UK", Disambiguation = "Description for artist one", Id = Guid.NewGuid(), Type = "Group" });
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Two", Country = "UK", Disambiguation = "Description for artist two", Id = Guid.NewGuid(), Type = "Singer" });
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Three", Country = "US", Disambiguation = "Description for artist three", Id = Guid.NewGuid(), Type = "Group" });
            artists.Artist.Add(new ArtistSummary() { Name = "Artist Four", Country = "JPN", Disambiguation = "Description for artist four", Id = Guid.NewGuid(), Type = "Composer" });

            _mockServiceClient
                .Setup(o => o.GetArtists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(artists));

            var sut = new DiscographyHelper(_mockLogger.Object, _mockServiceClient.Object, _mockWordCounter.Object, _mockNotificationHub.Object);
            var model = sut.ArtistSearch("Artist", page);

            Assert.AreEqual(totalArtistCount, model.Result.Results.Count);
            Assert.AreEqual(totalPageCount, model.Result.PageCount);
            Assert.AreEqual(page, model.Result.CurrentPage);
            Assert.IsTrue(model.Result.ShowPrevious);
            Assert.IsFalse(model.Result.ShowNext);
        }
    }
}
