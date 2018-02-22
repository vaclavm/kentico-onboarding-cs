﻿using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using NSubstitute;
using NUnit.Framework;

using ToDoList.Contracts.Services;

namespace ToDoList.API.Services.Tests
{
    [TestFixture]
    public class UrlLocationServiceTests
    {
        private IRoutesService _routesService;
        private UrlLocationService _locationService;

        private const string WhatsUp = "whatsUp";

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            configuration.Routes.MapHttpRoute("WhatsUp", $"{WhatsUp}/{{id}}", new { id = RouteParameter.Optional });

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = configuration;
            
            _routesService = Substitute.For<IRoutesService>();
            _locationService = new UrlLocationService(new UrlHelper(httpRequestMessage), _routesService);
        }

        [Test]
        public void GetNewResourceLocation_ReturnsCorrectLocation()
        {
            // Arrange
            string location = "WhatsUp";
            Guid guid = Guid.NewGuid();
            _routesService.ToDoRouteForGet.Returns(location);

            // Act
            var result = _locationService.GetNewResourceLocation(guid);

            // Assert
            Assert.That(result, Is.EqualTo($"/{WhatsUp}/{guid}"), $"The result location {result}, should be {WhatsUp}{guid}");
        }
    }
}
