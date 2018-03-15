using System.Linq;
using NUnit.Framework;

using ToDoList.Api.Tests.Utilities;
using ToDoList.Api.ViewModels;
using ToDoList.Contracts.Models;

namespace ToDoList.Api.Tests.ViewModels
{
    [TestFixture]
    public class ToDoViewModelTests
    {
        [Test]
        public void Validate_ValidViewModel_NoValidationReturned()
        {
            // Arrange
            var toDoViewModel = new ToDoViewModel { Text = "Test" };

            // Act
            var validations = toDoViewModel.Validate(null);

            // Assert
            Assert.That(validations, Is.Empty, "Validation rules should be empty");
        }

        [TestCase(" ")]
        [TestCase(null)]
        public void Validate_EmptyText_TextValidationReturned(string text)
        {
            // Arrange
            var toDoViewModel = new ToDoViewModel { Text = text };

            // Act
            var validations = toDoViewModel.Validate(null);
            var validationMessage = validations.FirstOrDefault();

            // Assert
            Assert.That(validationMessage, Is.Not.Null, "Validation should exist");
            Assert.That(validationMessage.MemberNames.FirstOrDefault(), Is.EqualTo(nameof(ToDoViewModel.Text)), "Validation for property Text missing");
        }

        [Test]
        public void Convert_ValidViewModel_ConvertedToDoReturned()
        {
            // Arrange
            var toDoViewModel = new ToDoViewModel { Text = "Test" };
            var expected = new ToDo { Text = toDoViewModel.Text };

            // Act
            var actual = toDoViewModel.Convert();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).UsingToDoComparer(), "Converted ToDo is not as expected");
        }
    }
}
