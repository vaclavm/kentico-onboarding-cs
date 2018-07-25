using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            var assembly = typeof(IDependencyRegister).Assembly;
            var excludedInterfaces = new [] { typeof(IContainer).FullName, typeof(IDependencyRegister).FullName };
            var dummyContainer = new DummyContainer();
            
            // Act
            var bootstrapper = new DependencyBootstrapper(dummyContainer);
            var resolver = bootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface && !type.IsGenericTypeDefinition);

            // Assert
            var unregistredInterfaces = interfaces
                .Where(type => !excludedInterfaces.Contains(type.FullName))
                .Where(@interface => resolver.GetService(@interface) == null)
                .Select(@interface => @interface.FullName)
                .ToArray();

            Assert.That(unregistredInterfaces, Is.Empty, $"{string.Join(", ", unregistredInterfaces)} don't have registred implementation");
        }
    }
}
