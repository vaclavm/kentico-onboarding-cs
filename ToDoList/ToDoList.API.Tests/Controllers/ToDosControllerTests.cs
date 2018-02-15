﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

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
            _controller = new ToDosController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            _toDoList = new List<ToDoModel>
            {
                new ToDoModel {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
                new ToDoModel {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
                new ToDoModel {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
            };
        }

        [Test]
        public void GetToDosAsync_AllToDosReturned()
        {
            // Act
            var response = _controller.GetToDosAsync().Result;
            var result = response as OkNegotiatedContentResult<List<ToDoModel>>;

            // Assert
            Assert.That(result, Is.Not.Null, $"Expecting status code OK, but was {response.GetType().Name}");
            Assert.That(result.Content, Is.EqualTo(_toDoList).Using(new ToDoComparer()), "Todos are not equal");
        }

        [Test]
        public void GetToDoAsync_CorrectToDoReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = _controller.GetToDoAsync(_toDoList[itemIndex].Id).Result;
            var result = response as OkNegotiatedContentResult<ToDoModel>;

            // Assert
            Assert.That(result, Is.Not.Null, $"Expecting status code OK, but was {response.GetType().Name}");
            Assert.That(result.Content, Is.EqualTo(_toDoList[itemIndex]).Using(new ToDoComparer()), $"{result.Content} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public void AddToDoAsync_IsAdded_NewToDoReturned()
        {
            // Arrange
            int itemIndex = 2;

            // Act
            var response = _controller.AddToDoAsync(_toDoList[itemIndex]).Result;
            var result = response as CreatedNegotiatedContentResult<ToDoModel>;

            // Assert
            Assert.That(result, Is.Not.Null, $"Expecting status code Created, but was {response.GetType().Name}");
            Assert.That(result.Location.ToString(), Is.EqualTo($"/{itemIndex}"), $"Location of new todo is not as expected, was {result.Location}");
            Assert.That(result.Content, Is.EqualTo(_toDoList[itemIndex]).Using(new ToDoComparer()), $"{result.Content} is not equal to expected {_toDoList[itemIndex]}");
        }

        [Test]
        public void ChangeToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = _controller.ChangeToDoAsync(_toDoList[itemIndex].Id, _toDoList[itemIndex]).Result;
            var result = response as StatusCodeResult;

            // Assert
            Assert.That(result, Is.Not.Null, $"Expecting response of type StatusCodeResult, but was {response.GetType().Name}");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {result.StatusCode}");
        }

        [Test]
        public void DeleteToDoAsync_IsDeleted_NoContentReturned()
        {
            // Arrange
            int itemIndex = 0;

            // Act
            var response = _controller.DeleteToDoAsync(_toDoList[itemIndex].Id).Result;
            var result = response as StatusCodeResult;

            // Assert
            Assert.That(result, Is.Not.Null, $"Expecting response of type StatusCodeResult, but was {response.GetType().Name}");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {result.StatusCode}");
        }
    }
}
