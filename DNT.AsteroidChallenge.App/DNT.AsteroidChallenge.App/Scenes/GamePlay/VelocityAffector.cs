using System;
using DNT.Engine.Core.Particles;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class VelocityAffector : IParticleAffector
    {
        public VelocityAffector(Vector3 velocityChange)
        {
            _velocityChange = velocityChange;
        }

        private readonly Vector3 _velocityChange;

        public void Affect(GameTime gameTime, Particle particle)
        {
            if (particle.Velocity.HasValue)
                particle.Velocity += (Single)gameTime.ElapsedGameTime.TotalSeconds * _velocityChange;
        }
    }
}