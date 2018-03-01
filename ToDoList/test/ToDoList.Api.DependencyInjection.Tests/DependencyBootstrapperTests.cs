using System.Linq;
using System.Reflection;
using NUnit.Framework;

using ToDoList.Api.DependencyInjection;
using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.API.Helpers;

namespace ToDoList.API.DependencyInjection.Tests
{
    [TestFixture]
    public class DependencyBootstrapperTests
    {
        [Test]
        public void CreateWebApiResolver_AllInstancesAreRegistered()
        {
            // Arrange
            var routeHelper = new WebApiRoutes();
            var assembly = Assembly.Load("ToDoList.Contracts");

            // Act
            DependencyResolver resolver = (DependencyResolver) DependencyBootstrapper.CreateWebApiResolver(routeHelper);
            var typesInAssembly = assembly.GetExportedTypes().Where(type => type.IsInterface);

            // Assert
            foreach (var type in typesInAssembly)
            {
                Assert.That(resolver.IsRegistered(type), Is.True, $"{type} is not registered");
            }
        }
    }
}
