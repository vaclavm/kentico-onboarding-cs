﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using NUnit.Framework;

using ToDoList.API.Controllers;
using ToDoList.API.Models;
using ToDoList.API.Tests.Utilities;

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
            var response = await _controller.ExecuteAction(controller => controller.GetToDosAsync());
            response.TryGetContentValue(out List<ToDoModel> result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList).UsingToDoComparer(), "Todos are not equal");
        }

        [Test]
        public async Task GetToDoAsync_CorrectToDoReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(_toDoList[itemIndex].Id));
            response.TryGetContentValue(out ToDoModel result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(_toDoList[itemIndex]).UsingToDoComparer(), $"{result} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public async Task PostToDoAsync_IsAdded_NewToDoWithEqualRouteReturned()
        {
            // Arrange
            int itemIndex = 2;

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(_toDoList[itemIndex]));
            response.TryGetContentValue(out ToDoModel result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo($"{_controller.Request.RequestUri}/{_toDoList[itemIndex].Id}"), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(_toDoList[itemIndex]).UsingToDoComparer(), $"{result} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public async Task PutToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(_toDoList[itemIndex].Id, _toDoList[itemIndex]));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
        }

        [Test]
        public async Task DeleteToDoAsync_IsDeleted_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(_toDoList[itemIndex].Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
        }
    }
}