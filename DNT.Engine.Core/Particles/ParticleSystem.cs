using System;
using System.Collections.Generic;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Particles
{
    public class ParticleSystem : DrawableComponent<Texture2D>
    {
        public ParticleSystem(Scene scene, BlendState blendState, Int32 maxCapacity, String assetName)
            : base(scene)
        {
            Verify.That(assetName).Named("assetName").IsNotNullOrWhiteSpace();
            _assetName = assetName;

            _blendState = blendState.IsNull() ? BlendState.NonPremultiplied : blendState;

            _liveParticles = new List<Particle>(maxCapacity);
            _deadParticles = new Stack<Particle>(maxCapacity);

            for (var i = 0; i < maxCapacity; i++)
                _deadParticles.Push(new Particle());

            _emitters = new List<IEmitter>();
            _affectors = new List<IParticleAffector>();
        }

        private readonly String _assetName;
        private readonly IList<IEmitter> _emitters;
        private readonly IList<IParticleAffector> _affectors;

        private BasicEffect _basicEffect;

        protected override Texture2D Content
        {
            get { return _content; }
        }
        private Texture2D _content;

        private Vector2 _textureOrigin;

        private readonly List<Particle> _liveParticles;
        private readonly Stack<Particle> _deadParticles;

        protected BlendState BlendState
        {
            get { return _blendState; }
        }
        private readonly BlendState _blendState;

        protected Boolean RemoveAt(Int32 index)
        {
            if (index < _liveParticles.Count)
            {
                _deadParticles.Push(_liveParticles[index]);
                _liveParticles[index] = _liveParticles[_liveParticles.Count - 1];
                _liveParticles.RemoveAt(_liveParticles.Count - 1);

                return true;
            }

            return false;
        }

        protected void ForEachLiveParticle(Action<Particle> function)
        {
            for (var i = 0; i < _liveParticles.Count; i++)
                if (function.IsNotNull())
                    function(_liveParticles[i]);
        }

        protected void Clear()
        {
            for (var i = _liveParticles.Count - 1; i >= 0; i--)
            {
                _deadParticles.Push(_liveParticles[i]);
                _liveParticles.RemoveAt(i);
            }
        }

        public override void Load()
        {
            base.Load();

            _basicEffect = new BasicEffect(Scene.GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
                LightingEnabled = false,
                World = Matrix.CreateScale(1.0f, -1.0f, 1.0f),
                View = Matrix.Identity
            };
            _content = Scene.Content.Load<Texture2D>(_assetName);
            _textureOrigin = new Vector2(_content.Width / 2.0f, _content.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateEmitters(gameTime);

            for (var i = 0; i < _liveParticles.Count; i++)
            {
                var particle = _liveParticles[i];

                particle.UpdateLifetime(gameTime);

                if (!particle.IsAlive)
                {
                    RemoveAt(i);
                    i--;
                    continue;
                }

                UpdateParticlePosition(particle);
                UpdateParticleRotation(particle);

                Affect(gameTime, particle);
            }
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            _basicEffect.Projection = Scene.CurrentCamera.Projection;

            Scene.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                    _blendState,
                                    null,
                                    DepthStencilState.DepthRead,
                                    RasterizerState.CullNone,
                                    _basicEffect);

            for (var i = 0; i < _liveParticles.Count; i++)
            {
                var particle = _liveParticles[i];
                var viewSpacePosition = GetParticleViewSpacePosition(particle);
                Scene.SpriteBatch.Draw(_content,
                                       new Vector2(viewSpacePosition.X, viewSpacePosition.Y),
                                       null,
                                       particle.Color,
                                       particle.Angle,
                                       _textureOrigin,
                                       particle.Scale,
                                       0,
                                       viewSpacePosition.Z);
            }

            Scene.SpriteBatch.End();
            Scene.GraphicsDevice.ResetRenderStates();
        }

        private Vector3 GetParticleViewSpacePosition(Particle particle)
        {
            return Vector3.Transform(particle.Position, Scene.CurrentCamera.View * Matrix.CreateScale(1.0f, -1.0f, 1.0f));
        }

        private void Affect(GameTime gameTime, Particle particle)
        {
            foreach (var affector in _affectors)
                affector.Affect(gameTime, particle);
        }

        private static void UpdateParticleRotation(Particle particle)
        {
            if (particle.Rotation.HasValue)
                particle.Angle += particle.Rotation.Value;
        }

        private static void UpdateParticlePosition(Particle particle)
        {
            if (particle.Velocity.HasValue)
                particle.Position += particle.Velocity.Value;
        }

        private void UpdateEmitters(GameTime gameTime)
        {
            foreach (var emitter in _emitters)
                emitter.Update(gameTime);
        }

        public void AddEmitter(IEmitter emitter)
        {
            _emitters.Add(emitter);
        }

        public void RemoveEmitter(IEmitter emitter)
        {
            _emitters.Remove(emitter);
        }

        public void AddAffector(IParticleAffector affector)
        {
            _affectors.Add(affector);
        }

        public void AddParticle(Vector3 position, Color color, Vector3? velocity, Single? rotation, TimeSpan? lifespan)
        {
            AddParticle(position, color, velocity, rotation, lifespan, 0.0f, 1.0f);
        }

        public void AddParticle(Vector3 position,
                                Color color,
                                Vector3? velocity,
                                Single? rotation,
                                TimeSpan? lifespan,
                                Single angle,
                                Single scale)
        {
            if (_deadParticles.Count == 0)
                return;

            var particle = _deadParticles.Pop();

            particle.InitialPosition = particle.Position = position;
            particle.InitialVelocity = particle.Velocity = velocity;
            particle.InitialColor = particle.Color = color;
            particle.InitialAngle = particle.Angle = angle;
            particle.InitialRotation = particle.Rotation = rotation;
            particle.InitialScale = particle.Scale = scale;
            particle.Lifespan = lifespan;

            _liveParticles.Add(particle);
        }
    }
}