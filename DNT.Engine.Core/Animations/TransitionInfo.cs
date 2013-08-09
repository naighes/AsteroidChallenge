using System;

namespace DNT.Engine.Core.Animations
{
    internal class TransitionInfo : ITransitionInfo
    {
        private readonly Single _initialValue;
        private readonly Single _finalValue;
        private TimeSpan _time;

        internal Func<Single, Single, Single, Single> InterpolationFunc
        {
            get { return _interpolationFunc; }
        }
        private Func<Single, Single, Single, Single> _interpolationFunc;

        internal TransitionInfo(Single initialValue, Single finalValue)
            : this(initialValue, finalValue, TimeSpan.Zero)
        {
            _initialValue = initialValue;
            _finalValue = finalValue;
        }

        protected TransitionInfo(Single initialValue, Single finalValue, TimeSpan time)
        {
            _initialValue = initialValue;
            _finalValue = finalValue;
            _time = time;
        }

        internal Single InitialValue
        {
            get { return _initialValue; }
        }

        internal Single FinalValue
        {
            get { return _finalValue; }
        }

        internal TimeSpan Time
        {
            get { return _time; }
        }

        public ITransitionInfo Within(TimeSpan time)
        {
            _time = time;
            return this;
        }

        public ITransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc)
        {
            _interpolationFunc = interpolationFunc;
            return this;
        }

        public virtual ILoopedTransitionInfo AsLoop()
        {
            return new LoopedTransitionInfo(this);
        }

        public virtual ITransition Instance()
        {
            return new Transition(_initialValue, _finalValue, _time, _interpolationFunc);
        }

        public IAuditableTransitionInfo Auditable()
        {
            return new AuditableTransitionInfo(this);
        }
    }
}