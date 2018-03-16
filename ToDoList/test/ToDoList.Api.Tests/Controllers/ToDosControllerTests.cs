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
using ToDoList.Api.ViewModels;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;
using ToDoList.Test.Utils;

namespace ToDoList.Api.Tests.Controllers
{
    [TestFixture]
    public class ToDosControllerTests
    {
        private static readonly IEnumerable<ToDo> ToDoList = new[]
        {
            new ToDo {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
            new ToDo {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
            new ToDo {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
        };

        private ILocator _locatorSubstitute;
        private IModificationService<ToDo> _modificationServiceSubstitute;
        private IRetrievalService<ToDo> _retrievalServiceSubstitute;
        private IToDoRepository _repositoryService;
        private ToDosController _controller;

        [SetUp]
        public void SetUp()
        {
            _locatorSubstitute = Substitute.For<ILocator>();
            _modificationServiceSubstitute = Substitute.For<IModificationService<ToDo>>();
            _retrievalServiceSubstitute = Substitute.For<IRetrievalService<ToDo>>();
            _repositoryService = Substitute.For<IToDoRepository>();

            _controller = new ToDosController(_locatorSubstitute, _modificationServiceSubstitute, _retrievalServiceSubstitute, _repositoryService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public async Task GetToDosAsync_OkReturned()
        {
            // Arrange
            _repositoryService.GetToDosAsync().Returns(ToDoList);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDosAsync());
            response.TryGetContentValue(out IEnumerable<ToDo> result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(ToDoList).UsingToDoComparer(), "Todos are not equal");
            Assert.That(() => _repositoryService.Received().GetToDosAsync(), Throws.Nothing, $"GetToDosAsync should have been called");
        }

        [Test]
        public async Task GetToDoAsync_Exists_OkReturned()
        {
            // Arrange
            const int itemIndex = 0;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);
            _retrievalServiceSubstitute.RetrieveOneAsync(expectedToDo.Id).Returns(expectedToDo);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(expectedToDo.Id));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Expecting status code OK, but was {response.StatusCode}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _retrievalServiceSubstitute.Received().RetrieveOneAsync(expectedToDo.Id), Throws.Nothing, $"RetrieveOneAsync should have been called");
        }

        [Test]
        public async Task GetToDoAsync_DoesNotExists_NotFoundReturned()
        {
            // Arrange
            const int itemIndex = 0;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.GetToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), $"Expecting status code NotFound, but was {response.StatusCode}");
            Assert.That(() => _retrievalServiceSubstitute.Received().RetrieveOneAsync(expectedToDo.Id), Throws.TypeOf<ReceivedCallsException>(), $"RetrieveOneAsync should not have been called");
        }

        [Test]
        public async Task PostToDoAsync_CorrectModel_CreatedReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            string location = $"todos/{expectedToDo.Id}";
            var postToDo = new ToDoViewModel { Text = expectedToDo.Text };
            
            _modificationServiceSubstitute.CreateAsync(Arg.Any<IConvertibleObject<ToDo>>()).Returns(expectedToDo);
            _locatorSubstitute.GetNewResourceLocation(expectedToDo.Id).Returns(location);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(postToDo));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo(location), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _modificationServiceSubstitute.Received().CreateAsync(Arg.Any<IConvertibleObject<ToDo>>()), Throws.Nothing, $"CreateAsync should have been called");
        }

        [Test]
        public async Task PostToDoAsync_EmptyText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel { Text = string.Empty };

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task PostToDoAsync_WhiteSpacesText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel { Text = "     " };

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task PostToDoAsync_NullText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel();

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PostToDoAsync(postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task PutToDoAsync_IsChanged_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            var putToDo = new ToDoViewModel {Text = expectedToDo.Text};
            
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);
            _retrievalServiceSubstitute.RetrieveOneAsync(expectedToDo.Id).Returns(expectedToDo);
            _modificationServiceSubstitute.UpdateAsync(expectedToDo, Arg.Any<IConvertibleObject<ToDo>>()).ReturnsForAnyArgs(expectedToDo);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(expectedToDo.Id, putToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
            Assert.That(() => _modificationServiceSubstitute.Received().UpdateAsync(expectedToDo, Arg.Any<IConvertibleObject<ToDo>>()), Throws.Nothing, $"UpdateAsync should have been called");
        }

        [Test]
        public async Task PutToDoAsync_DoesNotExist_NotFoundReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            var putToDo = new ToDoViewModel { Text = expectedToDo.Text };
            
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(expectedToDo.Id, putToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), $"Expecting status code NotFound, but was {response.StatusCode}");
        }

        [Test]
        public async Task PutToDoAsync_ToDoWithEmptyGuid_CreatedReturned()
        {
            // Arrange
            const int itemIndex = 2;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            string location = $"todos/{expectedToDo.Id}";
            var putToDo = new ToDoViewModel { Text = expectedToDo.Text };

            _modificationServiceSubstitute.CreateAsync(Arg.Any<IConvertibleObject<ToDo>>()).Returns(expectedToDo);
            _locatorSubstitute.GetNewResourceLocation(expectedToDo.Id).Returns(location);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(Guid.Empty, putToDo));
            response.TryGetContentValue(out ToDo result);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expecting status code Created, but was {response.StatusCode}");
            Assert.That(response.Headers.Location.ToString(), Is.EqualTo(location), $"Location of new todo is not as expected, was {response.Headers.Location}");
            Assert.That(result, Is.EqualTo(expectedToDo).UsingToDoComparer(), $"{result} is not equal to expected {expectedToDo}");
            Assert.That(() => _modificationServiceSubstitute.Received().CreateAsync(Arg.Any<IConvertibleObject<ToDo>>()), Throws.Nothing, $"CreateAsync should have been called");
        }

        [Test]
        public async Task PutToDoAsync_EmptyText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel { Text = string.Empty };

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(Guid.NewGuid(), postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task PutToDoAsync_WhiteSpacesText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel { Text = "     " };

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(Guid.NewGuid(), postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task PutToDoAsync_NullText_BadRequestReturned()
        {
            // Arrange
            var postToDo = new ToDoViewModel();

            // Act
            _controller.Validate(postToDo);
            var response = await _controller.ExecuteAction(controller => controller.PutToDoAsync(Guid.NewGuid(), postToDo));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), $"Expecting status code BadRequest, but was {response.StatusCode}");
        }

        [Test]
        public async Task DeleteToDoAsync_Exists_NoContentReturned()
        {
            // Arrange
            const int itemIndex = 1;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(true);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"Expecting status code NoContent, but is {response.StatusCode}");
            Assert.That(() => _repositoryService.Received().DeleteToDoAsync(expectedToDo.Id), Throws.Nothing, $"DeleteToDoAsync should have been called");
        }

        [Test]
        public async Task DeleteToDoAsync_DoesNotExists_NotFoundReturned()
        {
            // Arrange
            const int itemIndex = 1;
            var expectedToDo = ToDoList.ElementAt(itemIndex);
            _retrievalServiceSubstitute.IsInDatabaseAsync(expectedToDo.Id).Returns(false);

            // Act
            var response = await _controller.ExecuteAction(controller => controller.DeleteToDoAsync(expectedToDo.Id));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), $"Expecting status code NotFound, but is {response.StatusCode}");
            Assert.That(() => _repositoryService.Received().DeleteToDoAsync(expectedToDo.Id), Throws.TypeOf<ReceivedCallsException>(), $"DeleteToDoAsync should not have been called");
        }
    }
}
