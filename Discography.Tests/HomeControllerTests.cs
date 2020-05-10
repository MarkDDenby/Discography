using System;
using Discography.Controllers;
using Discography.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Discography.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_Logger()
        {
            try
            {
                var controller = new HomeController(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("logger", ex.ParamName);
            }
        }

        [TestMethod]
        public void HomeController_Index_Returns_Correctly_Populated_ViewModel()
        {
            var controller = new HomeController(_mockLogger.Object);
            var result = controller.Index() as ViewResult;
            var viewModel = result.Model as ArtistSearchModel;

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(0, viewModel.Results.Count);
            Assert.AreEqual(0, viewModel.PageCount);
            Assert.AreEqual(0, viewModel.ArtistCount);
            Assert.AreEqual(0, viewModel.CurrentPage);
            Assert.AreEqual(0, viewModel.PageCount);
            Assert.AreEqual(null, viewModel.ArtistName);
            Assert.IsFalse(viewModel.ShowNext);
            Assert.IsFalse(viewModel.ShowPrevious);
        }
    }
}
