using System;
using System.Threading.Tasks;
using ServiceClient.Models;

namespace ServiceClient.Contracts
{
    public interface IDiscographyServiceClient
    {
        Task<Lyrics> GetLyrics(string artistName, string trackName);
        Task<Artist> GetArtist(Guid artistId);
        Task<Artists> GetArtists(string artistName, int limit, int offset);
        Task<Releases> GetReleases(Guid artistId, int limit, int offset);
    }
}