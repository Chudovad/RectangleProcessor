using RectangleProcessor.Services;

namespace RectangleProcessorTests
{
    [TestFixture]
    public class LoggerServiceTests
    {
        private const string LogFilePath = "test_log.txt";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(LogFilePath))
            {
                File.Delete(LogFilePath);
            }
        }

        [Test]
        public void LogToFile_WritesMessageToFile()
        {
            // Arrange
            var logger = new LoggerService(logToFile: true, logFilePath: LogFilePath);
            string message = "Test log message";

            // Act
            logger.Log(message);

            // Assert
            string loggedMessage = File.ReadAllText(LogFilePath).Trim();
            Assert.That(loggedMessage, Is.EqualTo(message));
        }

        [Test]
        public void LogToConsole_WritesMessageToConsole()
        {
            // Arrange
            var logger = new LoggerService(logToFile: false);
            string message = "Test log message";

            // Redirect console output
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                logger.Log(message);

                // Assert
                string loggedMessage = sw.ToString().Trim();
                Assert.That(loggedMessage, Is.EqualTo(message));
            }
        }
    }
}
