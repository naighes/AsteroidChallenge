using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Animations;
using DNT.Engine.Core.CollisionsDetection;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Messaging;
using DNT.Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Environment = DNT.Engine.Core.Context.Environment;

namespace DNT.AsteroidChallenge.App
{
    public class Asteroid : SceneModel, ISolidBody, ICollidable
    {
        public Asteroid(Scene scene,
                        String assetName,
                        Vector3 rotationAmount,
                        Single scale,
                        TimeSpan lifetime,
                        ExplosionManager explosionManager)
            : base(scene, assetName)
        {
            _rotationAmount = rotationAmount;
            _lifetime = lifetime;
            _explosionManager = explosionManager;
            _transition = BuildTransition.Between(0.0f, 1.0f)
                                         .Within(TimeSpan.FromSeconds(0.8f))
                                         .InterpolateWith(MathHelper.Lerp)
                                         .Instance();
            _scale = scale;
            _world = Matrix.Identity;
        }

        private readonly TimeSpan _lifetime;
        private readonly ExplosionManager _explosionManager;
        private readonly Single _scale;

        private Vector3 _position;
        private Vector3 _velocity;

        public override Matrix World
        {
            get { return _world; }
        }
        private Matrix _world;

        public void CheckCollision(ICollidable target)
        {
            if (target is LaserBullet)
                CheckLaserBulletCollision((LaserBullet)target);

            if (target is Ship)
                CheckShipCollision((Ship)target);
        }

        private void CheckShipCollision(Ship ship)
        {
            var intersect = BoundingSphere.Intersects(ship.BoundingSphere);

            // Check for bounding sphere intersection first, in order
            // to optimize performance..
            if (!intersect)
                return;

            // If sphere intersects, than do a more accurate collision check (per-triangle).
            if (ship.HasAtLeastOneTriangleIntersectsBoundingSphere(BoundingSphere))
                ship.Destroy();
        }

        private void CheckLaserBulletCollision(LaserBullet laserBullet)
        {
            var intersection = laserBullet.CurrentRay.Intersects(BoundingSphere);

            if (!intersection.HasValue || intersection > laserBullet.CoveredDistance)
                return;

            _explosionManager.AddExplosion(_position, _velocity);
            Scene.SoundManager.PlaySoundEffect("sounds/Explosion");

            if (!_collected)
            {
                _collected = true;
                Messenger.Send(new Message<AsteroidDestroyedScore>(new AsteroidDestroyedScore(1)));
            }

            MarkForRemoval();
            laserBullet.MarkForRemoval();
        }

        private Boolean _collected;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdatePosition(gameTime);

            var totalElapsed = (Single)gameTime.TotalGameTime.TotalSeconds;
            var rotation = Matrix.CreateFromYawPitchRoll(_rotationAmount.Y*totalElapsed,
                                                         _rotationAmount.X*totalElapsed,
                                                         0.0f);
            _world = Matrix.CreateScale(_scale) * rotation * Matrix.CreateTranslation(_position);

            _lifespan += gameTime.ElapsedGameTime;
            _transition.Update(gameTime);

            if (_lifespan > _lifetime && IsOutsideFrustum)
                MarkForRemoval();
        }

        private void UpdatePosition(GameTime gameTime)
        {
            _position += _velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private TimeSpan _lifespan;
        private Vector3 _rotationAmount;
        private readonly ITransition _transition;

        protected override void OnBeginDraw()
        {
            base.OnBeginDraw();

            Scene.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            base.Render(view, projection);

#if DEBUG
            Engine.Core.Graphics.Utils.BoundingSphereRenderer.Render(BoundingSphere,
                                                                     Scene.GraphicsDevice,
                                                                     Scene.CurrentCamera.View,
                                                                     Scene.CurrentCamera.Projection,
                                                                     Color.Red,
                                                                     Id);
#endif

            Scene.GraphicsDevice.ResetRenderStates();
        }

        protected override void SetupEffect(Effect effect, Matrix view, Matrix projection, Matrix world)
        {
            base.SetupEffect(effect, view, projection, world);

            var e = effect as BasicEffect;

            if (e.IsNull())
                return;

            Environment.Current().SetLights(e);
            e.Alpha = _transition.CurrentTransitionAmount;
        }

        public override void Load()
        {
            base.Load();

#if DEBUG
            Engine.Core.Graphics.Utils.BoundingSphereRenderer.InitializeGraphics(Scene.GraphicsDevice, Id);
#endif
        }

        protected override Effect GetCustomEffect()
        {
            return null;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public Single Mass
        {
            get { throw new NotImplementedException(); }
        }

        public Single DragFactor
        {
            get { throw new NotImplementedException(); }
        }
    }
}