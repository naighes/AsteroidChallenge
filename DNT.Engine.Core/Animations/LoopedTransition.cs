using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Animations
{
    public class LoopedTransition : Transition
    {
        private readonly Boolean _mustReverse;

        internal LoopedTransition(Single initialValue,
                                  Single finalValue,
                                  TimeSpan time,
                                  Func<Single, Single, Single, Single> interpolationFunc,
                                  Boolean mustReverse)
            : base(initialValue, finalValue, time, interpolationFunc)
        {
            _mustReverse = mustReverse;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCurrentTransitionAmount(gameTime);

            if (!HasReachedFinalValue())
                return;

            if (_mustReverse)
                SwapInitialAndFinalValues();
            else
                Reset();
        }

        private void SwapInitialAndFinalValues()
        {
            var tmp = InitialValue;
            InitialValue = FinalValue;
            FinalValue = tmp;
        }
    }
}