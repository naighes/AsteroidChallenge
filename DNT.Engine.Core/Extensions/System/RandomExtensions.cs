using Microsoft.Xna.Framework;

namespace System
{
    public static class RandomExtensions
    {
        public static Single Next(this Random random, Single min, Single max)
        {
            return min + (Single)random.NextDouble() * (max - min);
        }

        public static Color NextColor(this Random random, Color min, Color max)
        {
            return Color.Lerp(min, max, (Single)random.NextDouble());
        }
    }
}