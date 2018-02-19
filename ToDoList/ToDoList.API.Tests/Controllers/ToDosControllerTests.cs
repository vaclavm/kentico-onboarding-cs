using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using NUnit.Framework;

using ToDoList.API.Controllers;
using ToDoList.API.Models;
using ToDoList.API.Tests.Comparers;

namespace ToDoList.API.Tests.Controllers
{
    [TestFixture]
    public class ToDosControllerTests
    {
        private ToDosController _controller;
        private List<ToDoModel> _toDoList;

        [SetUp]
        public void SetUp()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("GetToDo", "api/v1/todos/{id}", new {id = RouteParameter.Optional});

            var httpRequestMessage = new HttpRequestMessage {RequestUri = new Uri("http://localhost:51200/api/v1/todos")};

            _controller = new ToDosController
            {
                Request = httpRequestMessage,
                Configuration = httpConfiguration
            };

            _toDoList = new List<ToDoModel>
            {
                new ToDoModel {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
                new ToDoModel {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
                new ToDoModel {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
            };
        }

        [Test]
        public async Task GetToDosAsync_AllToDosReturned()
        {
            // Act
            var response = await _controller.GetToDosAsync();
            var controllerAction = await response.ExecuteAsync(CancellationToken.None);
            controllerAction.TryGetContentValue(out List<ToDoModel> result);

            // Assert
            Assert.That(controllerAction.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {controllerAction.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList).Using(new ToDoComparer()), "Todos are not equal");
        }

        [Test]
        public async Task GetToDoAsync_CorrectToDoReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.GetToDoAsync(_toDoList[itemIndex].Id);
            var controllerAction = await response.ExecuteAsync(CancellationToken.None);
            controllerAction.TryGetContentValue(out ToDoModel result);

            // Assert
            Assert.That(controllerAction.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {controllerAction.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList[itemIndex]).Using(new ToDoComparer()), $"{result} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public async Task PostToDoAsync_IsAdded_NewToDoWithEqualRouteReturned()
        {
            // Arrange
            int itemIndex = 2;

            // Act
            var response = await _controller.PostToDoAsync(_toDoList[itemIndex]);
            var controllerAction = await response.ExecuteAsync(CancellationToken.None);
            controllerAction.TryGetContentValue(out ToDoModel result);

            // Assert
            Assert.That(controllerAction.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {controllerAction.StatusCode}");
            Assert.That(controllerAction.Headers.Location.ToString(), Is.EqualTo($"{_controller.Request.RequestUri}/{_toDoList[itemIndex].Id}"), $"Location of new todo is not as expected, was {controllerAction.Headers.Location}");
            Assert.That(result, Is.EqualTo(_toDoList[itemIndex]).Using(new ToDoComparer()), $"{result} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public async Task PutToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.PutToDoAsync(_toDoList[itemIndex].Id, _toDoList[itemIndex]);
            var controllerAction = await response.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.That(controllerAction.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {controllerAction.StatusCode}");
        }

        [Test]
        public async Task DeleteToDoAsync_IsDeleted_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.DeleteToDoAsync(_toDoList[itemIndex].Id);
            var controllerAction = await response.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.That(controllerAction.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {controllerAction.StatusCode}");
        }
    }
}
