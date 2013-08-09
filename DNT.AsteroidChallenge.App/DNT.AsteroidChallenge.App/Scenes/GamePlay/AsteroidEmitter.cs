using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class AsteroidEmitter : Emitter
    {
        public AsteroidEmitter(Scene scene,
                               String[] assets,
                               IWorldObject target,
                               ExplosionManager explosionManager)
            : base(scene)
        {
            Verify.That(target).Named("target").IsNotNull();
            _target = target;

            _explosionManager = explosionManager;
            _assets = assets;
            _random = new Random();
        }

        private readonly String[] _assets;
        private readonly IWorldObject _target;
        private readonly ExplosionManager _explosionManager;
        private readonly Random _random;

        protected override void EmitInternal(Int32 count)
        {
            for (var i = 0; i < count; i++)
            {
                var asteroid = new Asteroid(Scene,
                                            _assets[_random.Next(0, _assets.Length)],
                                            GetRandomRotation(),
                                            _random.Next(0.2f, 2.0f),
                                            TimeSpan.FromSeconds(5.0d),
                                            _explosionManager);
                var position = GenerateRandomPosition();
                asteroid.SetPosition(position);

                var scalarVelocity = _random.Next(10.0f, 120.0f);
                var forward = Vector3.Normalize(_target.World.Translation - position);
                forward = Vector3.Transform(forward,
                                            Matrix.CreateFromYawPitchRoll(_random.Next(-0.2f, 0.2f),
                                                                          _random.Next(-0.2f, 0.2f),
                                                                          _random.Next(-0.2f, 0.2f)));
                asteroid.SetVelocity(forward * scalarVelocity);
                asteroid.SetMaterial(0, new Material
                                            {
                                                DiffuseColor = Vector3.One,
                                                EmissiveColor = Vector3.Zero,
                                                SpecularColor = Vector3.One,
                                                SpecularPower = 16.0f
                                            });
                Scene.AddComponent(asteroid, "Models");
            }
        }

        private Vector3 GetRandomRotation()
        {
            return _random.Next(0, 2) == 0
                       ? new Vector3(0.0f, (Single) _random.NextDouble(), 0.0f)
                       : new Vector3((Single) _random.NextDouble(), 0.0f, 0.0f);
        }

        public BoundingFrustum GetBoundingFrustum(Single nearPlaneDistance, Single farPlaneDistance)
        {
            var forward = Vector3.Normalize(_target.World.Forward) * farPlaneDistance;
            var up = Vector3.Normalize(_target.World.Up);
            var view = Matrix.CreateLookAt(_target.World.Translation, _target.World.Translation + forward, up);
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                 Scene.Viewport.AspectRatio,
                                                                 nearPlaneDistance,
                                                                 farPlaneDistance);

            var viewProjection = view * projection;
            return new BoundingFrustum(viewProjection);
        }

        private Vector3 GenerateRandomPosition()
        {
            var position = GetBoundingFrustum(0.1f, 500.0f).GenerateRandomPosition(() => (Single)_random.NextDouble());
            return position + Vector3.Normalize(position - _target.World.Translation) * 500.0f;
        }
    }
}