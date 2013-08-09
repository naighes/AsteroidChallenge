using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Particles
{
    public class Particle
    {
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector3 _position;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color;

        public Single Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        private Single _angle;

        public Single Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        private Single _scale;

        public Vector3 InitialPosition
        {
            get { return _initialPosition; }
            internal set { _initialPosition = value; }
        }
        private Vector3 _initialPosition;

        public Color InitialColor
        {
            get { return _initialColor; }
            internal set { _initialColor = value; }
        }
        private Color _initialColor;

        public Single InitialAngle
        {
            get { return _initialAngle; }
            internal set { _initialAngle = value; }
        }
        private Single _initialAngle;

        public Single InitialScale
        {
            get { return _initialScale; }
            internal set { _initialScale = value; }
        }
        private Single _initialScale;

        public Vector3? Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        private Vector3? _velocity;

        public Vector3? InitialVelocity
        {
            get { return _initialVelocity; }
            internal set { _initialVelocity = value; }
        }
        private Vector3? _initialVelocity;

        public Single? Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        private Single? _rotation;

        public Single? InitialRotation
        {
            get { return _initialRotation; }
            internal set { _initialRotation = value; }
        }
        private Single? _initialRotation;

        public TimeSpan? Lifespan
        {
            get { return _lifespan; }
            internal set
            {
                _lifespan = value;
                _remainingLifetime = _lifespan.HasValue ? _lifespan.Value : TimeSpan.Zero;
            }
        }
        private TimeSpan? _lifespan;

        public TimeSpan RemainingLifetime
        {
            get { return _remainingLifetime; }
        }
        private TimeSpan _remainingLifetime;

        public Single? Age
        {
            get
            {
                if (Lifespan.HasValue)
                    return (Single)RemainingLifetime.TotalSeconds / (Single)Lifespan.Value.TotalSeconds;

                return null;
            }
        }

        internal void UpdateLifetime(GameTime gameTime)
        {
            if (LivesForever || RemainingLifetime <= TimeSpan.Zero)
                return;

            _remainingLifetime -= gameTime.ElapsedGameTime;

            if (RemainingLifetime < TimeSpan.Zero)
                _remainingLifetime = TimeSpan.Zero;
        }

        internal Boolean IsAlive
        {
            get
            {
                return LivesForever
                           ? true
                           : RemainingLifetime.TotalMilliseconds > 0.0;
            }
        }

        internal Boolean LivesForever
        {
            get { return !Lifespan.HasValue; }
        }

        public void Affect(GameTime gameTime, IParticleAffector affector)
        {
            Verify.That(affector).Named("affector").IsNotNull();
            affector.Affect(gameTime, this);
        }
    }
}