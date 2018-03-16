using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Repositories;
using ToDoList.Services.Services;
using ToDoList.Test.Utils;

namespace ToDoList.Services.Tests.ToDoServices
{
    [TestFixture]
    public class ModificationServiceTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private IIdentifierProvider _idProvider;
        private ITimeProvider _timeProvider;
        private ModificationToDoService _toModificationToDoService;
        private Guid _guid;

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _idProvider = Substitute.For<IIdentifierProvider>();
            _timeProvider = Substitute.For<ITimeProvider>();

            _toModificationToDoService = new ModificationToDoService(_toDoRepositorySubstitute, _idProvider, _timeProvider);

            _guid = Guid.Parse("088b288d-d149-4598-a9c1-49fdb2b7bb4a");
        }

        [Test]
        public async Task CreateToDoAsync_NewToDo_ToDoIsCreated()
        {
            // Arrange
            var currentTime = DateTime.Now;
            var expectedToDo = new ToDo
            {
                Id = _guid,
                Text = "Test item",
                Created = currentTime,
                LastModified = currentTime
            };
            var convertibleToDo = Substitute.For<IConvertibleObject<ToDo>>();

            convertibleToDo.Convert().Returns(expectedToDo);
            _timeProvider.GetCurrentDateTime().Returns(currentTime);
            _idProvider.GenerateIdentifier().Returns(_guid);

            // Act
            var response = await _toModificationToDoService.CreateAsync(convertibleToDo);

            // Assert
            Assert.That(response, Is.EqualTo(expectedToDo).UsingToDoComparer(), "ToDo item is not as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received().AddToDoAsync(response), Throws.Nothing, $"AddToDoAsync should have been called");
        }

        [Test]
        public async Task UpdateToDoAsync_ToDoWithChangedText_TextAndLastModifiedIsUpdated()
        {
            // Arrange
            var textToUpdate = "Update item";
            var creationDateTime = DateTime.Now;
            var updatedDateTime = creationDateTime.AddDays(1);
            var existingToDo = new ToDo
            {
                Id = _guid,
                Text = "Test item",
                Created = creationDateTime,
                LastModified = creationDateTime

            };
            var expectedToDo = new ToDo
            {
                Id = _guid,
                Text = textToUpdate,
                Created = creationDateTime,
                LastModified = updatedDateTime

            };
            var convertibleToDo = Substitute.For<IConvertibleObject<ToDo>>();

            convertibleToDo.Convert().Returns(new ToDo { Text = textToUpdate });
            _timeProvider.GetCurrentDateTime().Returns(updatedDateTime);

            // Act
            var response = await _toModificationToDoService.UpdateAsync(existingToDo, convertibleToDo);

            // Assert
            Assert.That(response, Is.EqualTo(expectedToDo).UsingToDoComparer(), "ToDo item is not as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received().ChangeToDoAsync(response), Throws.Nothing, $"ChangeToDoAsync should have been called");
        }
    }
}
