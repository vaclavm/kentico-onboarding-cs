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
        public void CreateWebApiResolver_NotGenericInterfaces_AllAreRegistered()
        {
            // Arrange
            var routeHelper = new WebApiRoutes();
            var assembly = Assembly.Load("ToDoList.Contracts");

            // Act
            DependencyResolver resolver = (DependencyResolver) DependencyBootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface);

            // Assert
            foreach (var interfaceType in interfaces.Where(type => !type.IsGenericTypeDefinition))
            {
                Assert.That(resolver.IsRegistered(interfaceType), Is.True, $"{interfaceType} don't have registred implementation");
            }
        }
    }
}
