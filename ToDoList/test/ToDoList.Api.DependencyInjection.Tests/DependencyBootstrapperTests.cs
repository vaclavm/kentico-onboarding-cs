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
            var assembly = Assembly.Load("ToDoList.Contracts");
            var unregistredInterfaces = new [] { typeof(IContainer).FullName, typeof(IDependencyRegister).FullName };
            var dummyContainer = new DummyContainer();
            
            // Act
            var bootstrapper = new DependencyBootstrapper(dummyContainer);
            var resolver = bootstrapper.CreateWebApiResolver(routeHelper);
            var interfaces = assembly.GetExportedTypes().Where(type => type.IsInterface && !type.IsGenericTypeDefinition);

            // Assert
            var ungeristredInterfaces = new List<string>();
            foreach (var interfaceType in interfaces.Where(type => !unregistredInterfaces.Contains(type.FullName)))
            {
                if (resolver.GetService(interfaceType) == null)
                {
                    ungeristredInterfaces.Add(interfaceType.ToString());
                }
            }

            Assert.That(ungeristredInterfaces, Is.Empty, $"{string.Join(", ", ungeristredInterfaces)} don't have registred implementation");
        }
    }
}
