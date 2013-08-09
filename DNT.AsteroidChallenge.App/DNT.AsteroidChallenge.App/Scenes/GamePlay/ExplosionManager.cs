using System;
using DNT.Engine.Core.Particles;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class ExplosionManager
    {
        private readonly ParticleSystem _fireParticleSystem;
        private readonly ParticleSystem _smokeParticleSystem;

        public ExplosionManager(ParticleSystem fireParticleSystem, ParticleSystem smokeParticleSystem)
        {
            _fireParticleSystem = fireParticleSystem;
            _smokeParticleSystem = smokeParticleSystem;
            _random = new Random();
        }

        private readonly Random _random;

        public void AddExplosion(Vector3 position, Vector3 initialVelocity)
        {
            AddFire(initialVelocity, position, 8);
            AddSmoke(initialVelocity, position, 5);
        }

        private void AddFire(Vector3 initialVelocity, Vector3 position, Int32 particleCount)
        {
            for (var i = 0; i < particleCount; i++)
            {
                var velocity = initialVelocity * 0.02f + new Vector3(_random.Next(-30.0f, 30.0f),
                                                                     _random.Next(30.0f, -10.0f),
                                                                     _random.Next(-30.0f, 30.0f)) * 0.005f;
                _fireParticleSystem.AddParticle(position,
                                                _random.NextColor(Color.DarkGray, Color.Gray),
                                                velocity,
                                                _random.Next(-0.01f, 0.01f),
                                                TimeSpan.FromSeconds(_random.Next(1, 3)),
                                                _random.Next(0.0f, MathHelper.Pi),
                                                0.1f);
            }
        }

        private void AddSmoke(Vector3 initialVelocity, Vector3 position, Int32 particleCount)
        {
            for (var i = 0; i < particleCount; i++)
            {
                var velocity = initialVelocity * 0.02f + new Vector3(_random.Next(-50.0f, 50.0f),
                                                                     _random.Next(40.0f, -40.0f),
                                                                     _random.Next(-50.0f, 50.0f)) * 0.005f;
                _smokeParticleSystem.AddParticle(position,
                                                 _random.NextColor(Color.LightGray, Color.White),
                                                 velocity,
                                                 _random.Next(-0.01f, 0.01f),
                                                 TimeSpan.FromSeconds(1),
                                                 _random.Next(0.0f, MathHelper.Pi),
                                                 0.25f);
            }
        }
    }
}