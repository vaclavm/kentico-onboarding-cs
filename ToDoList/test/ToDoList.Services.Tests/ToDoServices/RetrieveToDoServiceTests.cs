using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Services.ToDoServices;

namespace ToDoList.Services.Tests.ToDoServices
{
    [TestFixture]
    public class RetrieveToDoServiceTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private RetriveToDoService _retrieveToDoService;

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _retrieveToDoService = new RetriveToDoService(_toDoRepositorySubstitute);
        }

        [Test]
        public async Task RetrieveAllAsync()
        {
            // Act
            await _retrieveToDoService.RetriveAllAsync();

            // Assert
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDosAsync(), Throws.Nothing, "GetToDosAsync should have been called");
        }

        [Test]
        public async Task RetrieveOneAsync_ReturnsCorrectToDo()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var expectedToDo = new ToDo { Id = guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(guid).Returns(expectedToDo);

            // Act
            var retrievedToDo = await _retrieveToDoService.RetriveOneAsync(guid);

            // Assert
            Assert.That(retrievedToDo, Is.EqualTo(expectedToDo), "ToDo was not returned as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDosAsync(), Throws.Nothing, "GetToDosAsync should have been called");

        }

        [Test]
        public async Task RetrieveOneAsync_ReturnsCachedToDo()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var expectedToDo = new ToDo { Id = guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(guid).Returns(expectedToDo);
            await _retrieveToDoService.RetriveOneAsync(guid);

            // Act
            var retrievedToDo = await _retrieveToDoService.RetriveOneAsync(guid);

            // Assert
            Assert.That(retrievedToDo, Is.EqualTo(expectedToDo), "ToDo was not returned as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received(1).GetToDoAsync(guid), Throws.Nothing, "GetToDosAsync should have been called only once");
        }

        [Test]
        public async Task IsInDatabaseAsync_Exists_ReturnsTrue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var expectedToDo = new ToDo { Id = guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(guid).Returns(expectedToDo);

            // Act
            bool isInDb = await _retrieveToDoService.IsInDatabaseAsync(guid);

            // Assert
            Assert.That(isInDb, Is.True, "ToDo should have exists");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDoAsync(guid), Throws.Nothing, "GetToDosAsync should have been called");
        }

        [Test]
        public async Task IsInDatabaseAsync_Cached_ReturnsTrue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var expectedToDo = new ToDo { Id = guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(guid).Returns(expectedToDo);
            await _retrieveToDoService.RetriveOneAsync(guid);

            // Act
            bool isInDb = await _retrieveToDoService.IsInDatabaseAsync(guid);

            // Assert
            Assert.That(isInDb, Is.True, "ToDo should have exists");
            Assert.That(() => _toDoRepositorySubstitute.Received(1).GetToDoAsync(guid), Throws.Nothing, "GetToDosAsync should have been called once");
        }

        [Test]
        public async Task IsInDatabaseAsync_NotExists_ReturnsFalse()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            bool isInDb = await _retrieveToDoService.IsInDatabaseAsync(guid);

            // Assert
            Assert.That(isInDb, Is.False, "ToDo should have not exists");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDoAsync(guid), Throws.Nothing, "GetToDosAsync should have been called");
        }
    }
}
