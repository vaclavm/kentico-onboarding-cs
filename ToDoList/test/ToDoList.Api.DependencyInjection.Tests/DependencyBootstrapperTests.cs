using System.Linq;
using System.Reflection;
using NUnit.Framework;

using ToDoList.Api.DependencyInjection;
using ToDoList.Api.Providers;
using ToDoList.API.DependencyInjection.Tests.Container;
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
            var dummyContainer = new DummyContainer();
            
            // Act
            var bootstrapper = new DependencyBootstrapper(dummyContainer);
            var resolver = bootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface && !type.IsGenericTypeDefinition);

            // Assert
            foreach (var interfaceType in interfaces.Where(type => !unregistredInterfaces.Contains(type.FullName)))
            {
                var service = resolver.GetService(interfaceType);
                Assert.That(service, Is.Not.Null, $"{interfaceType} don't have registred implementation");
            }
        }

        [Test]
        public void CreateWebApiResolver_GenericInterfaces_HaveAtLeastOneRegistration()
        {
            // Arrange
            var routeHelper = new WebApiRoutes();
            var assembly = Assembly.Load("ToDoList.Contracts");
            var dummyContainer = new DummyContainer();

            // Act
            var bootstrapper = new DependencyBootstrapper(dummyContainer);
            var resolver = bootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface && type.IsGenericTypeDefinition);

            // Assert
            foreach (var interfaceType in interfaces)
            {
                var services = resolver.GetServices(interfaceType);
                Assert.That(services, Is.Not.Null.Or.Empty, $"{interfaceType} don't have registred at least one implementation");
            }
        }
    }
}
