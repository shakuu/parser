﻿using Moq;
using NUnit.Framework;

using TestStack.FluentMVCTesting;

using Parser.Common.Utilities.Contracts;
using Parser.Data.Services.Contracts;
using Parser.Data.ViewModels.Live;
using Parser.MvcClient.Controllers;

namespace Parser.MvcClient.Tests.ControllersTests.LiveControllerTests
{
    [TestFixture]
    public class Index_Should
    {
        [Test]
        public void RenderCorrectViewWithCorrectViewModel()
        {
            // Arrange
            var liveService = new Mock<ILiveService>();
            var identityProvider = new Mock<IIdentityProvider>();

            var liveController = new LiveController(liveService.Object, identityProvider.Object);

            var expectedViewModel = new LiveStatisticsViewModel();
            liveService.Setup(s => s.GetLiveStatisticsViewModel(It.IsAny<string>())).Returns(expectedViewModel);

            // Act & Assert
            liveController
                .WithCallTo(c => c.Index())
                .ShouldRenderDefaultView()
                .WithModel<LiveStatisticsViewModel>(actualViewModel =>
                {
                    Assert.That(actualViewModel, Is.SameAs(expectedViewModel));
                });
        }
    }
}
