using System;
using NUnit.Framework;

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
