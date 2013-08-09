using System;

namespace DNT.Engine.Core.Animations
{
    public interface ILoopedTransitionInfo
    {
        ILoopedTransitionInfo WithReverse();
        ILoopedTransitionInfo Within(TimeSpan time);
        ITransition Instance();
        ILoopedTransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc);
    }
}