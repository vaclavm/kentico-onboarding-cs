using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Services;
using ToDoList.Contracts.Repositories;

namespace ToDoList.Services.Tests
{
    [TestFixture]

    public class FormationServiceTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private IIdentifierService _identifierServiceSubstitute;
        private IDateTimeService _dateTimeService;
        private FormationService _toFormationService;

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _identifierServiceSubstitute = Substitute.For<IIdentifierService>();
            _dateTimeService = Substitute.For<IDateTimeService>();

            _toFormationService = new FormationService(_toDoRepositorySubstitute, _identifierServiceSubstitute, _dateTimeService);
        }

        [Test]
        public async Task CreateToDoAsync_ToDoIsSetCorrectly()
        {
            // Arrange
            var expectedToDo = new ToDo { Text = "Test item" };
            var guid = Guid.NewGuid();
            var currentTime = DateTime.Now;

            _dateTimeService.GetCurrentDateTime().Returns(currentTime);
            _identifierServiceSubstitute.GenerateIdentifier().Returns(guid);

            // Act
            var response = await _toFormationService.CreateToDoAsync(expectedToDo);

            // Assert
            Assert.That(response.Text, Is.EqualTo(expectedToDo.Text), "ToDo item is not as expected");
            Assert.That(response.Id, Is.EqualTo(guid), "Id is not as expected");
            Assert.That(response.Created, Is.EqualTo(currentTime), "Created date is not as expected");
            Assert.That(response.LastModified, Is.EqualTo(currentTime), "Last modified date is not as expected");
        }
    }
}
