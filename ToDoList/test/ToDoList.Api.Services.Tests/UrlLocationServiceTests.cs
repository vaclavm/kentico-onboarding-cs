using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using NSubstitute;
using NUnit.Framework;

using ToDoList.Contracts.Services;

namespace ToDoList.Api.Services.Tests
{
    [TestFixture]
    public class UrlLocationServiceTests
    {
        private IWebApiRoutes _webApiRoutes;
        private UrlLocationService _locationService;

        private const string Route = "whatsUp";
        private const string RouteName = "RouteName";

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            configuration.Routes.MapHttpRoute(RouteName, $"{Route}/{{id}}/hello", new { id = RouteParameter.Optional });

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = configuration;
            
            _webApiRoutes = Substitute.For<IWebApiRoutes>();
            _locationService = new UrlLocationService(new UrlHelper(httpRequestMessage), _webApiRoutes);
        }

        [Test]
        public void GetNewResourceLocation_ReturnsCorrectLocation()
        {
            // Arrange
            Guid guid = Guid.Parse("088b288d-d149-4598-a9c1-49fdb2b7bb4a");
            _webApiRoutes.ToDoRouteForGet.Returns(RouteName);

            // Act
            var result = _locationService.GetNewResourceLocation(guid);

            // Assert
            Assert.That(result, Is.EqualTo($"/{Route}/{guid}/hello"), $"The result location {result}, should contain {guid} in the middle of the URL");
        }
    }
}
