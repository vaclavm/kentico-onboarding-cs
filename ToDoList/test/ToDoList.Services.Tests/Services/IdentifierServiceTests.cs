using System;
using NUnit.Framework;
using ToDoList.Services.Services;

namespace ToDoList.Services.Tests.Services
{
    [TestFixture]
    public class IdentifierServiceTests
    {
        [Test]
        public void GenerateIdentifier_NotEmptyGuid()
        {
            // Arrange
            var identifierService = new IdentifierService();

            // Act
            var actualIdentifier = identifierService.GenerateIdentifier();

            // Assert
            Assert.That(actualIdentifier, Is.Not.EqualTo(Guid.Empty), $"{actualIdentifier} should not be empty");
        }
    }
}
