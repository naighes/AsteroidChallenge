using System;

namespace DNT.Engine.Core.Animations
{
    public interface ITransitionInfo
    {
        ITransitionInfo Within(TimeSpan time);
        ILoopedTransitionInfo AsLoop();
        ITransition Instance();
        IAuditableTransitionInfo Auditable();
        ITransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc);
    }
}