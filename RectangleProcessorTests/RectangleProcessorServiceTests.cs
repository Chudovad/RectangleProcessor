using RectangleProcessor.Models;
using RectangleProcessor.Services;

namespace RectangleProcessorTests
{
    [TestFixture]
    public class RectangleProcessorServiceTests
    {
        private ILoggerService _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new LoggerService(logToFile: false);
        }

        [Test]
        public void GetMainRectangle_CalculatesCorrectly()
        {
            // Arrange
            List<Rectangle> rectangles = new List<Rectangle>
            {
                new Rectangle(System.Drawing.Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(System.Drawing.Color.Pink, new Point(2, 2), new Point(5, 5)),
                new Rectangle(System.Drawing.Color.Purple, new Point(-1, -1), new Point(4, 4)),
            };

            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, _logger);

            // Act
            Rectangle mainRect = processor.GetMainRectangle();

            // Assert
            Assert.That(mainRect.BotLeft.X, Is.EqualTo(-1));
            Assert.That(mainRect.BotLeft.Y, Is.EqualTo(-1));
            Assert.That(mainRect.TopRight.X, Is.EqualTo(5));
            Assert.That(mainRect.TopRight.Y, Is.EqualTo(5));
        }

        [Test]
        public void FilterRectangles_IncludesSpecifiedColors()
        {
            // Arrange
            List<Rectangle> rectangles = new List<Rectangle>
            {
                new Rectangle(System.Drawing.Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(System.Drawing.Color.Pink, new Point(2, 2), new Point(5, 5)),
                new Rectangle(System.Drawing.Color.Purple, new Point(-1, -1), new Point(4, 4)),
            };

            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, _logger);

            // Act
            List<Rectangle> filtered = processor.FilterRectangles(new[] { System.Drawing.Color.Green, System.Drawing.Color.Pink }, include: true);

            // Assert
            Assert.That(filtered.Count, Is.EqualTo(2));
            Assert.IsTrue(filtered.Exists(r => r.Color == System.Drawing.Color.Green));
            Assert.IsTrue(filtered.Exists(r => r.Color == System.Drawing.Color.Pink));
        }

        [Test]
        public void FilterRectangles_ExcludesSpecifiedColors()
        {
            // Arrange
            List<Rectangle> rectangles = new List<Rectangle>
            {
                new Rectangle(System.Drawing.Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(System.Drawing.Color.Pink, new Point(2, 2), new Point(5, 5)),
                new Rectangle(System.Drawing.Color.Purple, new Point(-1, -1), new Point(4, 4)),
            };

            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, _logger);

            // Act
            List<Rectangle> filtered = processor.FilterRectangles(new[] { System.Drawing.Color.Green, System.Drawing.Color.Pink }, include: false);

            // Assert
            Assert.That(filtered.Count, Is.EqualTo(1));
            Assert.IsTrue(filtered.Exists(r => r.Color == System.Drawing.Color.Purple));
        }

        [Test]
        public void ProcessRectangles_IgnoresOutOfBounds()
        {
            // Arrange
            List<Rectangle> rectangles = new List<Rectangle>
            {
                new Rectangle(System.Drawing.Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(System.Drawing.Color.Pink, new Point(2, 2), new Point(5, 5)),
                new Rectangle(System.Drawing.Color.Purple, new Point(-1, -1), new Point(4, 4)),
            };

            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, _logger);

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                processor.ProcessRectangles(new[] { System.Drawing.Color.Green, System.Drawing.Color.Pink }, includeFilter: true, ignoreOutOfBounds: true);

                // Assert
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Color=Green"));
                Assert.IsTrue(output.Contains("Color=Pink"));
                Assert.IsFalse(output.Contains("Color=Purple"));
            }
        }

        [Test]
        public void FilterRectangles_WhenNoRectanglesMatchFilter_ReturnsEmptyList()
        {
            // Arrange
            List<Rectangle> rectangles = new List<Rectangle>
            {
                new Rectangle(System.Drawing.Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(System.Drawing.Color.Pink, new Point(2, 2), new Point(5, 5)),
            };

            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, _logger);

            // Act
            List<Rectangle> filtered = processor.FilterRectangles(new[] { System.Drawing.Color.Purple }, include: true);

            // Assert
            Assert.That(filtered.Count, Is.EqualTo(0));
        }
    }
}
