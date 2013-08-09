using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Animations
{
    public class Transition : ITransition
    {
        internal Transition(Single initialValue,
                            Single finalValue,
                            TimeSpan time,
                            Func<Single, Single, Single, Single> interpolationFunc)
        {
            _currentTransitionAmount = _initialValue = initialValue;
            _finalValue = finalValue;
            _time = time;
            _interpolationFunc = interpolationFunc;
        }

        public Single CurrentTransitionAmount
        {
            get { return _currentTransitionAmount; }
        }
        private Single _currentTransitionAmount;

        private readonly TimeSpan _time;
        private readonly Func<Single, Single, Single, Single> _interpolationFunc;

        protected Single InitialValue
        {
            get { return _initialValue; }
            set
            {
                _currentTransitionAmount = value;
                _initialValue = value;
            }
        }
        private Single _initialValue;

        protected Single FinalValue
        {
            get { return _finalValue; }
            set { _finalValue = value; }
        }
        private Single _finalValue;

        protected Boolean IsCompleted;

        public virtual void Update(GameTime gameTime)
        {
            if (IsCompleted)
                return;

            UpdateCurrentTransitionAmount(gameTime);

            if (HasReachedFinalValue())
                IsCompleted = true;
        }

        private Single _delta;

        protected void UpdateCurrentTransitionAmount(GameTime gameTime)
        {
            _delta = _time == TimeSpan.Zero
                         ? _finalValue
                         : _delta + (Single) (gameTime.ElapsedGameTime.TotalMilliseconds/_time.TotalMilliseconds);

            _currentTransitionAmount = _interpolationFunc.IsNotNull()
                                           ? _interpolationFunc(_initialValue, _finalValue, _delta)
                                           : MathHelper.Lerp(_initialValue, _finalValue, _delta);
            _currentTransitionAmount = MathHelper.Clamp(_currentTransitionAmount,
                                                        Math.Min(_initialValue, _finalValue),
                                                        Math.Max(_initialValue, _finalValue));

            if (HasReachedFinalValue())
                _delta = 0.0f;
        }

        protected Boolean HasReachedFinalValue()
        {
            return _currentTransitionAmount == _finalValue;
        }

        public void Reset()
        {
            _currentTransitionAmount = _initialValue;
            IsCompleted = false;
        }
    }
}