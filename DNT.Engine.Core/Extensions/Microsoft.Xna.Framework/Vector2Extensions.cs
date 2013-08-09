using System;

namespace Microsoft.Xna.Framework
{
    public static class Vector2Extensions
    {
        public static Boolean IsHover(this Vector2 source, Rectangle rectangle)
        {
            return source.X >= rectangle.Left && source.X <= rectangle.Right &&
                   source.Y >= rectangle.Top && source.Y <= rectangle.Bottom;
        }

        public static Vector3 ToVector3(this Vector2 source)
        {
            return new Vector3(source, 0.0f);
        }

        public static Point ToPoint(this Vector2 source)
        {
            return new Point((Int32)source.X, (Int32)source.Y);
        }
    }
}