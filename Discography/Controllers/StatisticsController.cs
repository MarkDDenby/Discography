using System;
using System.Threading;
using System.Threading.Tasks;
using Discography.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Discography.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IDiscographyHelper _discography;

        public StatisticsController(IDiscographyHelper discography)
        {
            _discography = discography ?? throw new ArgumentNullException(nameof(discography));
        }

        [HttpGet]
        public async Task<ActionResult> ArtistStatistics(Guid artistId, CancellationToken cancellationToken)
        {
            var model = await _discography.GetArtistStatistics(artistId, cancellationToken);

            return View(model);
        }
    }
}