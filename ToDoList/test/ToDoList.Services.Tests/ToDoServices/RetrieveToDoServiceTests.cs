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
        private RetrievalToDoService _retrievalToDoService;
        private Guid _guid;

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _retrievalToDoService = new RetrievalToDoService(_toDoRepositorySubstitute);
            _guid = Guid.Parse("088b288d-d149-4598-a9c1-49fdb2b7bb4a");
        }

        [Test]
        public async Task RetrieveOneAsync_ReturnsCorrectToDo()
        {
            // Arrange
            var expectedToDo = new ToDo { Id = _guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(_guid).Returns(expectedToDo);

            // Act
            var retrievedToDo = await _retrievalToDoService.RetrieveOneAsync(_guid);

            // Assert
            Assert.That(retrievedToDo, Is.EqualTo(expectedToDo), "ToDo was not returned as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDoAsync(_guid), Throws.Nothing, "GetToDosAsync should have been called");

        }

        [Test]
        public async Task RetrieveOneAsync_ReturnsCachedToDo()
        {
            // Arrange
            var expectedToDo = new ToDo { Id = _guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(_guid).Returns(expectedToDo);
            await _retrievalToDoService.RetrieveOneAsync(_guid);

            // Act
            var retrievedToDo = await _retrievalToDoService.RetrieveOneAsync(_guid);

            // Assert
            Assert.That(retrievedToDo, Is.EqualTo(expectedToDo), "ToDo was not returned as expected");
            Assert.That(() => _toDoRepositorySubstitute.Received(1).GetToDoAsync(_guid), Throws.Nothing, "GetToDosAsync should have been called only once");
        }

        [Test]
        public async Task IsInDatabaseAsync_Exists_ReturnsTrue()
        {
            // Arrange
            var expectedToDo = new ToDo { Id = _guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(_guid).Returns(expectedToDo);

            // Act
            bool isInDb = await _retrievalToDoService.IsInDatabaseAsync(_guid);

            // Assert
            Assert.That(isInDb, Is.True, "ToDo should have exists");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDoAsync(_guid), Throws.Nothing, "GetToDosAsync should have been called");
        }

        [Test]
        public async Task IsInDatabaseAsync_Cached_ReturnsTrue()
        {
            // Arrange
            var expectedToDo = new ToDo { Id = _guid, Text = "Test" };
            _toDoRepositorySubstitute.GetToDoAsync(_guid).Returns(expectedToDo);
            await _retrievalToDoService.RetrieveOneAsync(_guid);

            // Act
            bool isInDb = await _retrievalToDoService.IsInDatabaseAsync(_guid);

            // Assert
            Assert.That(isInDb, Is.True, "ToDo should have exists");
            Assert.That(() => _toDoRepositorySubstitute.Received(1).GetToDoAsync(_guid), Throws.Nothing, "GetToDosAsync should have been called once");
        }

        [Test]
        public async Task IsInDatabaseAsync_NotExists_ReturnsFalse()
        {
            // Act
            bool isInDb = await _retrievalToDoService.IsInDatabaseAsync(_guid);

            // Assert
            Assert.That(isInDb, Is.False, "ToDo should have not exists");
            Assert.That(() => _toDoRepositorySubstitute.Received().GetToDoAsync(_guid), Throws.Nothing, "GetToDosAsync should have been called");
        }
    }
}
