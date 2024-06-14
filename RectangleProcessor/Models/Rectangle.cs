using System.Drawing;

namespace RectangleProcessor.Models
{
    public class Rectangle
    {
        public Color Color { get; set; }
        public Point BotLeft { get; set; }
        public Point TopRight { get; set; }

        public Rectangle(Color color, Point botLeft, Point topRight)
        {
            Color = color;
            BotLeft = botLeft;
            TopRight = topRight;
        }
    }
}
