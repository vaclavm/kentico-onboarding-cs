using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ToDoList.Services.Providers;

namespace ToDoList.Services.Tests.Services
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        [Test]
        public void GetCurrentDateTime_ReturnsDateTimeNow()
        {
            // Arrange
            var dateTimeService = new TimeProvider();
            var expectedDateTime = DateTime.Now;

            // Act
            var actualDateTime = dateTimeService.GetCurrentDateTime();

            // Assert
            Assert.That(actualDateTime, Is.InRange(expectedDateTime, expectedDateTime.AddSeconds(5)), $"{actualDateTime} should be equal or greater (max by 5 seconds) to {expectedDateTime}");
        }
    }
}
