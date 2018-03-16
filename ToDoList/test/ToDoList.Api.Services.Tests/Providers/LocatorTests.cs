using System;
using System.Web.Http.Routing;
using NSubstitute;
using NUnit.Framework;
using ToDoList.Api.Services.Providers;
using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Services.Tests.Providers
{
    [TestFixture]
    public class UrlLocationServiceTests
    {
        private const string Route = "whatsUp";
        private const string RouteName = "RouteName";

        private UrlHelper _urlHelperSubstitute;
        private IWebApiRoutes _webApiRoutes;
        private Locator _locator;

        [SetUp]
        public void SetUp()
        {
            _urlHelperSubstitute = Substitute.For<UrlHelper>();
            _webApiRoutes = Substitute.For<IWebApiRoutes>();

            _locator = new Locator(_urlHelperSubstitute, _webApiRoutes);
        }

        [Test]
        public void GetNewResourceLocation_ReturnsCorrectLocation()
        {
            // Arrange
            Guid guid = Guid.Parse("088b288d-d149-4598-a9c1-49fdb2b7bb4a");
            var actualLocation = $"/{Route}/{guid}/hello";
            _webApiRoutes.ToDoRouteForGet.Returns(RouteName);
            _urlHelperSubstitute.Route(RouteName, Arg.Any<object>()).Returns(actualLocation);

            // Act
            var result = _locator.GetNewResourceLocation(guid);

            // Assert
            Assert.That(result, Is.EqualTo(actualLocation), $"The result location {result}, should contain {guid} in the middle of the URL");
        }
    }
}
