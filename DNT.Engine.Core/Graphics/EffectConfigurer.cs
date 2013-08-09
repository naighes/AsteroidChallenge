using System;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public class EffectConfigurer : IEffectConfigurer
    {
        public void ApplyMaterial(Effect effect, Material material)
        {
            if (effect is BasicEffect)
                ApplyMaterial((BasicEffect)effect, material);
            else if (effect is EnvironmentMapEffect)
                ApplyMaterial((EnvironmentMapEffect)effect, material);
            else if (effect is DualTextureEffect)
                ApplyMaterial((DualTextureEffect)effect, material);
            else if (effect is SkinnedEffect)
                ApplyMaterial((SkinnedEffect)effect, material);
            else
                throw new NotSupportedException();
        }

        private static void ApplyMaterial(BasicEffect effect, Material material)
        {
            effect.DiffuseColor = material.DiffuseColor;
            effect.EmissiveColor = material.EmissiveColor;
            effect.SpecularColor = material.SpecularColor;
            effect.SpecularPower = material.SpecularPower;

            if (material.Texture.IsNull() || effect.Texture.IsNotNull())
                return;

            effect.Texture = material.Texture;
            effect.TextureEnabled = true;
        }

        private static void ApplyMaterial(EnvironmentMapEffect effect, Material material)
        {
            effect.DiffuseColor = material.DiffuseColor;
            effect.EmissiveColor = material.EmissiveColor;

            if (material.Texture.IsNotNull() && effect.Texture.IsNull())
                effect.Texture = material.Texture;
        }

        private static void ApplyMaterial(DualTextureEffect effect, Material material)
        {
            effect.DiffuseColor = material.DiffuseColor;

            if (material.Texture.IsNotNull() && effect.Texture.IsNull())
                effect.Texture = material.Texture;
        }

        private static void ApplyMaterial(SkinnedEffect effect, Material material)
        {
            effect.DiffuseColor = material.DiffuseColor;
            effect.EmissiveColor = material.EmissiveColor;
            effect.SpecularColor = material.SpecularColor;
            effect.SpecularPower = material.SpecularPower;

            if (material.Texture.IsNotNull() && effect.Texture.IsNull())
                effect.Texture = material.Texture;
        }
    }
}