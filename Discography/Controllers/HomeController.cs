using System;
using Discography.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Discography.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("HomeController - Index");

            return View(new ArtistSearchModel());
        }

        [HttpPost]
        public ActionResult Index(ArtistSearchModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Results", "Search", new { artistName = model.ArtistName });
            }
            return View(model);
        }
    }
}