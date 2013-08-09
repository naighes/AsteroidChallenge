namespace Microsoft.Xna.Framework.Graphics
{
    public static class ViewportExtensions
    {
        public static Vector2 GetCenter(this Viewport viewport)
        {
            return new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
        }
    }
}