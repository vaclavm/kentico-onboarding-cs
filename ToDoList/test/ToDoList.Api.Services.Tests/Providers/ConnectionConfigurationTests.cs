using NUnit.Framework;
using ToDoList.Api.Services.Providers;

namespace ToDoList.Api.Services.Tests.Providers
{
    [TestFixture]
    public class ConnectionConfigurationServiceTests
    {
        [Test]
        public void ConnectionString()
        {
            // Act
            var service = new ConnectionConfiguration();
            
            // Assert
            Assert.That(service.ConnectionString, Is.EqualTo("http://test.url/todolist"), "Connection string is not correctly retrieved");
        }
    }
}
