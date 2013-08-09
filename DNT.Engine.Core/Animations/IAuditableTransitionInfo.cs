using System;

namespace DNT.Engine.Core.Animations
{
    public interface IAuditableTransitionInfo
    {
        IAuditableTransitionInfo Within(TimeSpan time);
        IAuditableTransition Instance();
        IAuditableTransitionInfo InterpolateWith(Func<Single, Single, Single, Single> interpolationFunc);
    }
}