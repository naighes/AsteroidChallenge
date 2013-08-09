using System;

namespace DNT.Engine.Core.Animations
{
    internal class LoopedTransitionInfo : ILoopedTransitionInfo
    {
        private readonly TransitionInfo _transitionInfo;

        internal LoopedTransitionInfo(TransitionInfo transitionInfo)
        {
            _transitionInfo = transitionInfo;
        }

        public ILoopedTransitionInfo WithReverse()
        {
            _reverse = true;
            return this;
        }

        public ILoopedTransitionInfo Within(TimeSpan time)
        {
            _transitionInfo.Within(time);
            return this;
        }

        public ITransition Instance()
        {
            return new LoopedTransition(_transitionInfo.InitialValue,
                                        _transitionInfo.FinalValue,
                                        _transitionInfo.Time,
                                        _transitionInfo.InterpolationFunc,
                                        _reverse);
        }

        public ILoopedTransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc)
        {
            _transitionInfo.InterpolateWith(interpolationFunc);
            return this;
        }

        private Boolean _reverse;
    }
}