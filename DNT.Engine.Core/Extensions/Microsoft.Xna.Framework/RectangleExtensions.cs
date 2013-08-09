using System;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtensions
    {
        public static Single GetHorizontalCenter(this Rectangle source, Rectangle destination)
        {
            return destination.Center.X - source.Width / 2.0f;
        }

        public static Single GetVerticalCenter(this Rectangle source, Rectangle destination)
        {
            return destination.Center.Y - source.Height / 2.0f;
        }

        public static Boolean IsEmpty(this Rectangle rectangle)
        {
            return rectangle == Rectangle.Empty;
        }

        public static Boolean IsNotEmpty(this Rectangle rectangle)
        {
            return rectangle != Rectangle.Empty;
        }
    }
}