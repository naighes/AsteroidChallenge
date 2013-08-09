using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Particles;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.AsteroidChallenge.App
{
    public class RandomStarsParticleSystem : ParticleSystem
    {
        public RandomStarsParticleSystem(Scene scene, BlendState blendState, Int32 maxCapacity, String assetName)
            : base(scene, blendState, maxCapacity, assetName)
        {
        }
    }
}