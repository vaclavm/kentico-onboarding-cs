using System;
using NUnit.Framework;
using ToDoList.Services.Providers;

namespace ToDoList.Services.Tests.Providers
{
    [TestFixture]
    public class IdentifierServiceTests
    {
        [Test]
        public void GenerateIdentifier_NotEmptyGuid()
        {
            // Arrange
            var identifierService = new IdentifierProvider();

            // Act
            var actualIdentifier = identifierService.GenerateIdentifier();

            // Assert
            Assert.That(actualIdentifier, Is.Not.EqualTo(Guid.Empty), $"{actualIdentifier} should not be empty");
        }
    }
}
