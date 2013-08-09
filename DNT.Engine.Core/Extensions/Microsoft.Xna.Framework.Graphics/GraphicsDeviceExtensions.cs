namespace Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtensions
    {
        public static void ResetRenderStates(this GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }
    }
}