using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;
using ToDoList.Services.ToDoServices;

namespace ToDoList.Services.Tests.ToDoServices
{
    [TestFixture]
    public class ModificationServiceTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private IIdentifierService _identifierServiceSubstitute;
        private IDateTimeService _dateTimeService;
        private ModificationToDoService _toModificationToDoService;

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _identifierServiceSubstitute = Substitute.For<IIdentifierService>();
            _dateTimeService = Substitute.For<IDateTimeService>();

            _toModificationToDoService = new ModificationToDoService(_toDoRepositorySubstitute, _identifierServiceSubstitute, _dateTimeService);
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
            var response = await _toModificationToDoService.CreateAsync(expectedToDo);

            // Assert
            Assert.That(response.Text, Is.EqualTo(expectedToDo.Text), "ToDo item is not as expected");
            Assert.That(response.Id, Is.EqualTo(guid), "Id is not as expected");
            Assert.That(response.Created, Is.EqualTo(currentTime), "Created date is not as expected");
            Assert.That(response.LastModified, Is.EqualTo(currentTime), "Last modified date is not as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received().AddToDoAsync(response), Throws.Nothing, $"AddToDoAsync should have been called");
        }

        [Test]
        public async Task UpdateToDoAsync_LastModifiedIsUpdated()
        {
            // Arrange
            var creationDateTime = DateTime.Now;
            var updatedDateTime = creationDateTime.AddDays(1);
            var expectedToDo = new ToDo { Id = Guid.NewGuid(), Text = "Test item", Created  = creationDateTime, LastModified = creationDateTime };

            _dateTimeService.GetCurrentDateTime().Returns(updatedDateTime);

            // Act
            var response = await _toModificationToDoService.UpdateAsync(expectedToDo);

            // Assert
            Assert.That(response.LastModified, Is.EqualTo(updatedDateTime), "Last modified date is not as expected");
            Assert.That(response.Created, Is.EqualTo(expectedToDo.Created), "Created date has changed");
            Assert.That(response.Text, Is.EqualTo(expectedToDo.Text), "Text has changed");
            Assert.That(response.Id, Is.EqualTo(expectedToDo.Id), "Id has changed");
            Assert.That(() => _toDoRepositorySubstitute.Received().ChangeToDoAsync(response), Throws.Nothing, $"AddToDoAsync should have been called");
        }

        [Test]
        public async Task UpdateToDoAsync_OriginalFieldsUnchanged()
        {
            // Arrange
            var creationDateTime = DateTime.Now;
            var expectedToDo = new ToDo { Id = Guid.NewGuid(), Text = "Test item", Created = creationDateTime, LastModified = creationDateTime };

            _dateTimeService.GetCurrentDateTime().Returns(creationDateTime.AddDays(1));

            // Act
            var response = await _toModificationToDoService.UpdateAsync(expectedToDo);

            // Assert
            Assert.That(response.Created, Is.EqualTo(expectedToDo.Created), "Created date has changed");
            Assert.That(response.Text, Is.EqualTo(expectedToDo.Text), "Text has changed");
            Assert.That(response.Id, Is.EqualTo(expectedToDo.Id), "Id has changed");
            Assert.That(() => _toDoRepositorySubstitute.Received().ChangeToDoAsync(response), Throws.Nothing, $"AddToDoAsync should have been called");
        }

        [Test]
        public async Task DeleteToDoAsync_IsDeletedAndReturned()
        {
            // Arrange
            var creationDateTime = DateTime.Now;
            var expectedToDo = new ToDo { Id = Guid.NewGuid(), Text = "Test item", Created = creationDateTime, LastModified = creationDateTime };
            
            // Act
            var response = await _toModificationToDoService.DeleteAsync(expectedToDo);

            // Assert
            Assert.That(response, Is.EqualTo(expectedToDo), "ToDo is not correctly returned");
            Assert.That(() => _toDoRepositorySubstitute.Received().DeleteToDoAsync(expectedToDo.Id), Throws.Nothing, $"DeleteToDoAsync should have been called");
        }
    }
}
