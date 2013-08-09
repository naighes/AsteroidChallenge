namespace Microsoft.Xna.Framework.Graphics
{
    public static class EffectExtensions
    {
        public static void TryToExtractTexture(this Effect effect, out Texture2D texture)
        {
            texture = null;

            if (effect is BasicEffect)
                texture = ((BasicEffect)effect).Texture;

            if (effect is EnvironmentMapEffect)
                texture = ((EnvironmentMapEffect)effect).Texture;

            if (effect is DualTextureEffect)
                texture = ((DualTextureEffect)effect).Texture;

            if (effect is SkinnedEffect)
                texture = ((SkinnedEffect)effect).Texture;
        }
    }
}