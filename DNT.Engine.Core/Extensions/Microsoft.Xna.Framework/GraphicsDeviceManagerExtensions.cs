namespace Microsoft.Xna.Framework
{
    public static class GraphicsDeviceManagerExtensions
    {
        public static void SetPortraitMode(this GraphicsDeviceManager graphicsDeviceManager)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = 480;
            graphicsDeviceManager.PreferredBackBufferHeight = 800;
            graphicsDeviceManager.SupportedOrientations = DisplayOrientation.Portrait;
        }

        public static void SetLandscapeMode(this GraphicsDeviceManager graphicsDeviceManager)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = 800;
            graphicsDeviceManager.PreferredBackBufferHeight = 480;
            graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }
    }
}