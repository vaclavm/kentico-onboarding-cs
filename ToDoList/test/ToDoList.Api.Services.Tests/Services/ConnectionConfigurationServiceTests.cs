using NUnit.Framework;

using ToDoList.Api.Services.Services;

namespace ToDoList.Api.Services.Tests.Services
{
    [TestFixture]
    public class ConnectionConfigurationServiceTests
    {
        [Test]
        public void ConnectionString()
        {
            // Act
            var service = new ConnectionConfigurationService();
            
            // Assert
            Assert.That(service.ConnectionString, Is.EqualTo("http://test.url/todolist"), "Connection string is not correctly retrieved");
        }
    }
}
