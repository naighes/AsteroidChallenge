using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.AsteroidChallenge.App.Scenes.GamePlay
{
    public class ExplosionSmokeParticleSystem : ParticleSystem
    {
        public ExplosionSmokeParticleSystem(Scene scene, BlendState blendState, Int32 maxCapacity, String assetName)
            : base(scene, blendState, maxCapacity, assetName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ForEachLiveParticle(particle =>
                                    {
                                        particle.Color = Color.Lerp(particle.InitialColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f - particle.Age.Value);
                                        particle.Scale += 0.0004f;
                                    });
        }
    }
}