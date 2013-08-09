using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public interface IEffectConfigurer
    {
        void ApplyMaterial(Effect effect, Material material);
    }
}