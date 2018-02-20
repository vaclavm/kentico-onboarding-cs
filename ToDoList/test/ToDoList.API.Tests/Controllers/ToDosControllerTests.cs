using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;

using ToDoList.API.Controllers;
using ToDoList.API.Tests.Utilities;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.API.Tests.Controllers
{
    [TestFixture]
    public class ToDosControllerTests
    {
        private IToDoRepository _toDoRepositorySubstitute;
        private IUrlLocationService _urlLocationServiceSubstitute;
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

            _controller = new ToDosController(_toDoRepositorySubstitute, _urlLocationServiceSubstitute)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public async Task GetToDosAsync_AllToDosReturned()
        {
            // Arrange
            _toDoRepositorySubstitute.GetToDosAsync().Returns(_toDoList);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDosAsync());
            response.TryGetContentValue(out IEnumerable<ToDo> result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList).UsingToDoComparer(), "Todos are not equal");
        }

        [Test]
        public async Task GetToDoAsync_CorrectToDoReturned()
        {
            // Arrange
            const int itemIndex = 0;
            var expectedToDo = _toDoList.ElementAt(itemIndex);
            _toDoRepositorySubstitute.GetToDoAsync(expectedToDo.Id).Returns(expectedToDo);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(expectedToDo.Id));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
        }

        [Test]
        public async Task PostToDoAsync_IsAdded_NewToDoWithEqualRouteReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = _toDoList.ElementAt(itemIndex);

            _toDoRepositorySubstitute.AddToDoAsync(expectedToDo).Returns(expectedToDo);
            _urlLocationServiceSubstitute.GetAfterPostLocation(expectedToDo.Id).Returns($"todos/{expectedToDo.Id}");

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(expectedToDo));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo($"todos/{expectedToDo.Id}"), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
        }

        [Test]
        public async Task PutToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = _toDoList.ElementAt(itemIndex);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(expectedToDo.Id, expectedToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
        }

        [Test]
        public async Task DeleteToDoAsync_IsDeleted_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 1;
            var expectedToDo = _toDoList.ElementAt(itemIndex);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
        }
    }
}
