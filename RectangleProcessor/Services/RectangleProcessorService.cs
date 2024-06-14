using System.Drawing;

namespace RectangleProcessor.Services
{
    public class RectangleProcessorService : IRectangleProcessorService
    {
        private readonly List<Models.Rectangle> _rectangles;
        private Models.Rectangle _mainRectangle;
        private readonly ILoggerService _logger;

        public RectangleProcessorService(List<Models.Rectangle> rectangles, ILoggerService logger)
        {
            _rectangles = rectangles;
            _logger = logger;
            CalculateMainRectangle();
        }

        private void CalculateMainRectangle()
        {
            if (_rectangles.Count == 0)
            {
                _logger.Log("No rectangles to process.");
                return;
            }

            double minX = _rectangles.Min(r => r.BotLeft.X);
            double minY = _rectangles.Min(r => r.BotLeft.Y);
            double maxX = _rectangles.Max(r => r.TopRight.X);
            double maxY = _rectangles.Max(r => r.TopRight.Y);

            _mainRectangle = new Models.Rectangle(Color.Black, new Models.Point(minX, minY), new Models.Point(maxX, maxY));

            _logger.Log($"Main rectangle calculated: Bottom Left ({minX}, {minY}), Top Right ({maxX}, {maxY})");
        }

        public Models.Rectangle GetMainRectangle()
        {
            return _mainRectangle;
        }

        public List<Models.Rectangle> FilterRectangles(Color[] colors, bool include)
        {
            List<Models.Rectangle> filteredRectangles;

            if (include)
            {
                filteredRectangles = _rectangles.Where(r => colors.Contains(r.Color)).ToList();
            }
            else
            {
                filteredRectangles = _rectangles.Where(r => !colors.Contains(r.Color)).ToList();
            }

            _logger.Log($"Filtered rectangles count: {filteredRectangles.Count}");
            return filteredRectangles;
        }

        public void ProcessRectangles(Color[] filterColors, bool includeFilter, bool ignoreOutOfBounds)
        {
            List<Models.Rectangle> processedRectangles = FilterRectangles(filterColors, includeFilter);

            if (ignoreOutOfBounds)
            {
                processedRectangles = processedRectangles.Where(r =>
                    r.BotLeft.X >= _mainRectangle.BotLeft.X && r.BotLeft.Y >= _mainRectangle.BotLeft.Y &&
                    r.TopRight.X <= _mainRectangle.TopRight.X && r.TopRight.Y <= _mainRectangle.TopRight.Y).ToList();
            }

            foreach (var rect in processedRectangles)
            {
                _logger.Log($"Rectangle: Color={rect.Color.Name}, Bottom Left=({rect.BotLeft.X}, {rect.BotLeft.Y}), Top Right=({rect.TopRight.X}, {rect.TopRight.Y})");
            }
        }
    }
}
