using System;
using DNT.Engine.Core;
using DNT.Engine.Core.CollisionsDetection;
using DNT.Engine.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.AsteroidChallenge.App
{
    public class LaserBullet : AnimatedSprite, ICollidable, IBoundable
    {
        public LaserBullet(Scene scene,
                           String assetName,
                           Vector3 position,
                           Vector3 velocity)
            : base(scene, assetName)
        {
            _position = position;
            _velocity = velocity;

            _lifeTime = TimeSpan.FromSeconds(1.5d);
            _world = Matrix.Identity;
        }

        private Matrix _world;
        private BasicEffect _basicEffect;
        private Vector3 _position;
        private readonly Vector3 _velocity;
        private const Single Scale = 0.05f;
        private readonly TimeSpan _lifeTime;
        private TimeSpan _elapsedTime;

        public Ray CurrentRay
        {
            get { return _currentRay; }
        }
        private Ray _currentRay;

        public Single CoveredDistance
        {
            get { return _coveredDistance; }
        }
        private Single _coveredDistance;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateLifeTime(gameTime);
            UpdatePosition(gameTime);

            _world = Matrix.CreateScale(Scale) *
                     Matrix.CreateBillboard(_position, Scene.CurrentCamera.Position, Scene.CurrentCamera.Up * -1.0f, null);
        }

        private void UpdateLifeTime(GameTime gameTime)
        {
            if (_elapsedTime >= _lifeTime)
                MarkForRemoval();
            else
                _elapsedTime += gameTime.ElapsedGameTime;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            _currentRay = new Ray(_position, Vector3.Normalize(_velocity));
            var previousPosition = _position;
            _position += _velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;
            _coveredDistance = (_position - previousPosition).Length();
        }

        protected override void OnBeginDraw()
        {
            base.OnBeginDraw();

            SetDepthStencilState(DepthStencilState.DepthRead);
            SetRasterizerState(RasterizerState.CullNone);
            UseCustomEffect(_basicEffect);
            _basicEffect.TextureEnabled = true;
            SetEffectMatrices(_basicEffect, Scene.CurrentCamera.View, Scene.CurrentCamera.Projection, _world);

            if (CurrentAnimation.IsNotNull())
                SetOrigin(new Vector2(SourceRectangle.Width / 2.0f,
                                      SourceRectangle.Height / 2.0f));
        }

        protected override Rectangle SourceRectangle
        {
            get
            {
                var rectangle = base.SourceRectangle;
                return new Rectangle(0, 0, rectangle.Width, rectangle.Height);
            }
        }

        public BoundingSphere BoundingSphere
        {
            get { return new BoundingSphere(_position, SourceRectangle.Width * Scale / 2.0f); }
        }

        public override void Load()
        {
            base.Load();

            _basicEffect = new BasicEffect(Scene.GraphicsDevice);
        }

        public Matrix World
        {
            get { return _world; }
        }

        public void CheckCollision(ICollidable target)
        {
            /*
            var intersection = _currentRay.Intersects(target.BoundingSphere);

            if (intersection.HasValue && intersection <= _coveredDistance)
            {
                // Collided!
            }
            */
        }

        protected Boolean IsOutsideFrustum
        {
            get { return Scene.CurrentCamera.BoundingFrustum.Contains(_position) == ContainmentType.Disjoint; }
        }
    }
}