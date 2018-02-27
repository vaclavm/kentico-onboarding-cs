using System.IO;
using System.Net.Http;
using System.Web;
using NUnit.Framework;

using ToDoList.API.Helpers;
using ToDoList.API.Services;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;
using ToDoList.Repository;

namespace ToDoList.API.DependencyInjection.Tests
{
    [TestFixture]
    public class DependencyBootstrapperTests
    {
        [Test]
        public void CreateWebApiResolver_AllInstancesAreRegistered()
        {
            // Arrange
            var routeHelper = new RoutesHelper();
            var mockContext = new HttpContext(new HttpRequest(string.Empty, "http://www.google.com", string.Empty), new HttpResponse(new StringWriter()));
            mockContext.Items["MS_HttpRequestMessage"] = new HttpRequestMessage();
            HttpContext.Current = mockContext;

            // Act
            var resolver = DependencyBootstrapper.CreateWebApiResolver(routeHelper);
            var routeService = resolver.GetService(typeof(RoutesHelper));
            var toDoRepository = resolver.GetService(typeof(ToDoRepository));
            var urlLocaltionService = resolver.GetService(typeof(UrlLocationService));

            // Assert
            Assert.That(routeService, Is.InstanceOf<IRoutesService>());
            Assert.That(toDoRepository, Is.InstanceOf<IToDoRepository>());
            Assert.That(urlLocaltionService, Is.InstanceOf<IUrlLocationService>());
        }
    }
}
