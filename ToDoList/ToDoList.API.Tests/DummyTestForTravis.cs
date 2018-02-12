using NUnit.Framework;

namespace ToDoList.API.Tests
{
    [TestFixture]
    public class DummyTestForTravis
    {
        [Test]
        public void DummyTest_OnePlusOne_ReturnCorrectValue()
        {
            // Arrange, Act,
            int result = 1 + 1;

            // Assert
            Assert.AreEqual(result, 2, "Addition does not work correctly");
        }
    }
}
