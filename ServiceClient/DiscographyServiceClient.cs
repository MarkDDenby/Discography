using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceClient.Contracts;
using ServiceClient.Models;

namespace ServiceClient
{
    public class DiscographyServiceClient : ServiceClientBase, IDiscographyServiceClient
    {
        private const string ApiRoot = "http://musicbrainz.org/ws/2/";
        private const string LyricsApiRoot = "https://api.lyrics.ovh/v1/";

        public DiscographyServiceClient(IHttpClientFactory httpClientFactory, ILogger<ServiceClientBase> logger) : base(httpClientFactory, logger)
        {
            Headers.Add("user-agent", "Discography / 1.0.0 (markddenby@hotmail.com)");
        }

        public async Task<Lyrics> GetLyrics(string artistName, string trackName)
        {
            string url = $"{LyricsApiRoot}{artistName}/{trackName}";

            HttpResponseMessage response =  await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<Lyrics>();
            }

            return null;
        }

        public async Task<Artist> GetArtist(Guid artistId)
        {
            string url = $"{ApiRoot}artist/{artistId}?fmt=json";

            HttpResponseMessage response = await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<Artist>();
            }

            return null;
        }

        public async Task<Artists> GetArtists(string artistName, int limit, int offset)
        {
            string url = $"{ApiRoot}artist/?query={artistName}&fmt=json&limit={limit}&offset={offset}";

            HttpResponseMessage response = await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<Artists>();
            }

            return null;
        }

        public async Task<Releases> GetReleases(Guid artistId, int limit, int offset)
        {
            string url = $"{ApiRoot}release/?artist={artistId}&inc=recordings&status=official&fmt=json&limit={limit}&offset={offset}";

            HttpResponseMessage response = await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<Releases>();
            }

            return null;
        }
    }
}