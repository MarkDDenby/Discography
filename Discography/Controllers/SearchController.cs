using System;
using System.Threading.Tasks;
using Discography.Contracts;
using Discography.Models;
using Microsoft.AspNetCore.Mvc;

namespace Discography.Controllers
{
    public class SearchController : Controller
    {
        private readonly IDiscographyHelper _discography;

        public SearchController(IDiscographyHelper discography)
        {
            _discography = discography ?? throw new ArgumentNullException(nameof(discography));
        }

        [HttpGet]
        public async Task<IActionResult> Results(string artistName, int currentPage = 1)
        {
            var model = await _discography.ArtistSearch(artistName, currentPage);

            return View(model);
        }

        [HttpPost]
        public ActionResult NextPage(ArtistSearchModel model)
        {
            model.CurrentPage++;

            return RedirectToAction("Results", new { artistName = model.ArtistName, currentPage = model.CurrentPage });
        }

        [HttpPost]
        public ActionResult PreviousPage(ArtistSearchModel model)
        {
            model.CurrentPage--;

            return RedirectToAction("Results", new { artistName = model.ArtistName, currentPage = model.CurrentPage });
        }
    }
}