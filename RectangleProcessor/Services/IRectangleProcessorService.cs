using System.Drawing;

namespace RectangleProcessor.Services
{
    public interface IRectangleProcessorService
    {
        Models.Rectangle GetMainRectangle();
        List<Models.Rectangle> FilterRectangles(Color[] colors, bool include);
        void ProcessRectangles(Color[] filterColors, bool includeFilter, bool ignoreOutOfBounds);
    }
}
