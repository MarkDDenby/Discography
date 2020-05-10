using System;
using System.Threading;
using System.Threading.Tasks;
using Discography.Models;

namespace Discography.Contracts
{
    public interface IDiscographyHelper
    {
        Task<ArtistSearchModel> ArtistSearch(string artistName, int currentPage);
        Task<ArtistTrackStatisticsModel> GetArtistStatistics(Guid artistId, CancellationToken cancellationToken);
    }
}
