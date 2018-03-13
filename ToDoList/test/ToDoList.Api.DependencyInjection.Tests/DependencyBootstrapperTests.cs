using System.Linq;
using System.Reflection;
using NUnit.Framework;

using ToDoList.Api.DependencyInjection;
using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.API.Helpers;
using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.API.DependencyInjection.Tests
{
    [TestFixture]
    public class DependencyBootstrapperTests
    {
        [Test]
        public void CreateWebApiResolver_NotGenericInterfaces_AreRegistered()
        {
            // Arrange
            var routeHelper = new WebApiRoutes();
            var assembly = Assembly.Load("ToDoList.Contracts");
            var unregistredInterfaces = new [] { typeof(IContainer).FullName, typeof(IDependencyRegister).FullName };

            // Act
            DependencyResolver resolver = (DependencyResolver) DependencyBootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface && !type.IsGenericTypeDefinition);

            // Assert
            foreach (var interfaceType in interfaces.Where(type => !unregistredInterfaces.Contains(type.FullName)))
            {
                Assert.That(resolver.IsRegistered(interfaceType), Is.True, $"{interfaceType} don't have registred implementation");
            }
        }
    }
}
