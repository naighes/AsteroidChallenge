using System;
using DNT.Engine.Core;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public abstract class Weapon : SceneComponent
    {
        private readonly TimeSpan _chargeTime;
        private Int32 _munitions;
        private readonly IWorldObject _source;
        private TimeSpan _elapsedSinceLastShot;
        private Boolean _enabled = true;

        protected Weapon(Scene scene, TimeSpan chargeTime, Int32 munitions, IWorldObject source)
            : base(scene)
        {
            _chargeTime = chargeTime;
            _munitions = munitions;
            _source = source;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Shot()
        {
            if (!CanShot())
                return;

            Shot(_source);
            DecreaseMunitions();
            ResetTimer();
        }

        private void ResetTimer()
        {
            _elapsedSinceLastShot = TimeSpan.Zero;
        }

        private void DecreaseMunitions()
        {
            if (_munitions > 0 && _munitions != -1)
                _munitions--;
        }

        protected abstract void Shot(IWorldObject source);

        private Boolean CanShot()
        {
            return (_munitions > 0 || _munitions == -1) &&
                   (_elapsedSinceLastShot >= _chargeTime || _chargeTime == TimeSpan.MaxValue) &&
                   _enabled;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _elapsedSinceLastShot += gameTime.ElapsedGameTime;
        }
    }
}