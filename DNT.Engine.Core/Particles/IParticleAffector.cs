using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Particles
{
    public interface IParticleAffector
    {
        void Affect(GameTime gameTime, Particle particle);
    }
}