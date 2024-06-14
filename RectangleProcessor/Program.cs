using RectangleProcessor.Services;
using System.Drawing;

namespace RectangleProcessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Models.Rectangle> rectangles = new List<Models.Rectangle>
            {
                new Models.Rectangle(Color.Green, new Models.Point(1, 1), new Models.Point(3, 3)),
                new Models.Rectangle(Color.Pink, new Models.Point(2, 2), new Models.Point(5, 5)),
                new Models.Rectangle(Color.Purple, new Models.Point(-1, -1), new Models.Point(4, 4)),
            };

            ILoggerService logger = new LoggerService(logToFile: false);
            IRectangleProcessorService processor = new RectangleProcessorService(rectangles, logger);

            processor.ProcessRectangles(new[] { Color.Green, Color.Pink }, includeFilter: true, ignoreOutOfBounds: false);

            Models.Rectangle mainRect = processor.GetMainRectangle();
            logger.Log($"Main Rectangle: Bottom Left ({mainRect.BotLeft.X}, {mainRect.BotLeft.Y}), Top Right ({mainRect.TopRight.X}, {mainRect.TopRight.Y})");
        }
    }
}
