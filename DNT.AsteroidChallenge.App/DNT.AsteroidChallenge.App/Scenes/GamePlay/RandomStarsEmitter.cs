using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Particles;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class RandomStarsEmitter : Emitter
    {
        public RandomStarsEmitter(Scene scene, ParticleSystem particleSystem)
            : base(scene)
        {
            _particleSystem = particleSystem;
            _random = new Random();
        }

        private readonly ParticleSystem _particleSystem;

        private readonly Random _random;

        protected override void EmitInternal(Int32 count)
        {
            for (var i = 0; i < count; i++)
            {
                var velocity = Scene.CurrentCamera.Forward*_random.Next(-45.0f, -15.0f);
                _particleSystem.AddParticle(GenerateRandomPosition(),
                                            Color.White,
                                            Vector3.Transform(velocity,
                                                              Matrix.CreateFromYawPitchRoll(_random.Next(-0.2f, 0.2f),
                                                                                            _random.Next(-0.2f, 0.2f),
                                                                                            _random.Next(-0.2f, 0.2f))),
                                            0.0f,
                                            TimeSpan.FromSeconds(15.0d),
                                            0.0f,
                                            _random.Next(0.01f, 0.05f));
            }
        }

        private Vector3 GenerateRandomPosition()
        {
            var position = Scene.CurrentCamera.BoundingFrustum.GenerateRandomPosition(() => (Single)_random.NextDouble());
            return position + Vector3.Normalize(position - Scene.CurrentCamera.Position) * 500.0f;
        }
    }
}