using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;

using ToDoList.Api.Controllers;
using ToDoList.Api.Tests.Utilities;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Tests.Controllers
{
    [TestFixture]
    public class ToDosControllerTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private IUrlLocationService _urlLocationServiceSubstitute;
        private IFormationService _formationServiceSubstitute;
        private IRetrieveService<ToDo> _retrieveServiceSubstitute;
        private ToDosController _controller;

        private static readonly IEnumerable<ToDo> _toDoList = new[]
        {
            new ToDo {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
            new ToDo {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
            new ToDo {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
        };

        [SetUp]
        public void SetUp()
        {
            _toDoRepositorySubstitute = Substitute.For<IToDoRepository>();
            _urlLocationServiceSubstitute = Substitute.For<IUrlLocationService>();
            _formationServiceSubstitute = Substitute.For<IFormationService>();
            _retrieveServiceSubstitute = Substitute.For<IRetrieveService<ToDo>>();

            _controller = new ToDosController(_toDoRepositorySubstitute, _urlLocationServiceSubstitute, _formationServiceSubstitute, _retrieveServiceSubstitute)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public async Task GetToDosAsync_OkReturned()
        {
            // Arrange
            _retrieveServiceSubstitute.RetriveAllAsync().Returns(_toDoList);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDosAsync());
            response.TryGetContentValue(out IEnumerable<ToDo> result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList).UsingToDoComparer(), "Todos are not equal");
            Assert.That(() => _retrieveServiceSubstitute.Received().RetriveAllAsync(), Throws.Nothing, $"RetriveAllAsync should have been called");
        }

        [Test]
        public async Task GetToDoAsync_Exists_OkReturned()
        {
            // Arrange
            const int itemIndex = 0;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);
            _retrieveServiceSubstitute.RetriveOneAsync(expectedToDo.Id).Returns(expectedToDo);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(expectedToDo.Id));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _retrieveServiceSubstitute.Received().RetriveOneAsync(expectedToDo.Id), Throws.Nothing, $"RetriveOneAsync should have been called");
        }

        [Test]
        public async Task GetToDoAsync_DoesNotExists_NotFoundReturned()
        {
            // Arrange
            const int itemIndex = 0;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), $"Expecting status code NotFound, but was {response.StatusCode}");
            Assert.That(() => _retrieveServiceSubstitute.Received().RetriveOneAsync(expectedToDo.Id), Throws.TypeOf<ReceivedCallsException>(), $"RetriveOneAsync should not have been called");
        }

        [Test]
        public async Task PostToDoAsync_IsAdded_CreatedReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            string location = $"todos/{expectedToDo.Id}";

            _formationServiceSubstitute.CreateToDoAsync(expectedToDo).Returns(expectedToDo);
            _urlLocationServiceSubstitute.GetNewResourceLocation(expectedToDo.Id).Returns(location);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(expectedToDo));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo(location), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _formationServiceSubstitute.Received().CreateToDoAsync(expectedToDo), Throws.Nothing, $"CreateToDoAsync should have been called");
        }

        [Test]
        public async Task PutToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);

            _formationServiceSubstitute.UpdateToDoAsync(expectedToDo).Returns(expectedToDo);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(expectedToDo.Id, expectedToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
            Assert.That(() => _formationServiceSubstitute.Received().UpdateToDoAsync(expectedToDo), Throws.Nothing, $"UpdateToDoAsync should have been called");
            Assert.That(() => _formationServiceSubstitute.Received().CreateToDoAsync(expectedToDo), Throws.TypeOf<ReceivedCallsException>(), $"CreateToDoAsync should not have been called");
        }

        [Test]
        public async Task PutToDoAsync_DoesNotExist_CreatedReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            string location = $"todos/{expectedToDo.Id}";
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            _formationServiceSubstitute.CreateToDoAsync(expectedToDo).Returns(expectedToDo);
            _urlLocationServiceSubstitute.GetNewResourceLocation(expectedToDo.Id).Returns(location);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(expectedToDo.Id, expectedToDo));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo(location), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _formationServiceSubstitute.Received().CreateToDoAsync(expectedToDo), Throws.Nothing, $"CreateToDoAsync should have been called");
            Assert.That(() => _formationServiceSubstitute.Received().UpdateToDoAsync(expectedToDo), Throws.TypeOf<ReceivedCallsException>(), $"UpdateToDoAsync should not have been called");
        }

        [Test]
        public async Task DeleteToDoAsync_Exists_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 1;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
            Assert.That(() => _toDoRepositorySubstitute.Received().DeleteToDoAsync(expectedToDo.Id), Throws.Nothing, $"DeleteToDoAsync should have been called");
        }

        [Test]
        public async Task DeleteToDoAsync_DoesNotExists_NotFoundReturned()
        {
            // Arrange
            const int itemIndex = 1;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _retrieveServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), $"Expecting status code NotFound, but is {response.StatusCode}");
            Assert.That(() => _toDoRepositorySubstitute.Received().DeleteToDoAsync(expectedToDo.Id), Throws.TypeOf<ReceivedCallsException>(), $"DeleteToDoAsync should not have been called");
        }
    }
}
