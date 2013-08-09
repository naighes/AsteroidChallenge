using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public abstract class Emitter : SceneComponent, IEmitter
    {
        protected Emitter(Scene scene)
            : base(scene)
        {
            _threshold = TimeSpan.FromSeconds(1.0d);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _elapsed += gameTime.ElapsedGameTime;

            if (_elapsed <= _threshold)
                return;

            Emit(_itemsPerTime());
            _elapsed = TimeSpan.Zero;
            _threshold = _every();
        }

        public void Emit(Int32 particlesToEmit)
        {
            if (_emitting)
                EmitInternal(particlesToEmit);
        }

        protected abstract void EmitInternal(Int32 particlesToEmit);

        private Boolean _emitting;

        public void Start()
        {
            _emitting = true;
        }

        public void Stop()
        {
            _emitting = false;
        }

        public Emitter EmitEvery(Func<TimeSpan> func)
        {
            _every = func;
            return this;
        }

        public Emitter ItemsPerTime(Func<Int32> func)
        {
            _itemsPerTime = func;
            return this;
        }

        private TimeSpan _elapsed;
        private TimeSpan _threshold;
        private Func<TimeSpan> _every;
        private Func<Int32> _itemsPerTime;
    }
}