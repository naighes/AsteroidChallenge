using System;

namespace DNT.Engine.Core.Animations
{
    internal class AuditableTransitionInfo : IAuditableTransitionInfo
    {
        private readonly TransitionInfo _transitionInfo;

        public AuditableTransitionInfo(TransitionInfo transitionInfo)
        {
            _transitionInfo = transitionInfo;
        }

        public IAuditableTransitionInfo Within(TimeSpan time)
        {
            _transitionInfo.Within(time);
            return this;
        }

        public IAuditableTransition Instance()
        {
            return new AuditableTransition(_transitionInfo.InitialValue,
                                           _transitionInfo.FinalValue,
                                           _transitionInfo.Time,
                                           _transitionInfo.InterpolationFunc);
        }

        public IAuditableTransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc)
        {
            _transitionInfo.InterpolateWith(interpolationFunc);
            return this;
        }
    }
}