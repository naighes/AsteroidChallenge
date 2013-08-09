using System;

namespace DNT.Engine.Core.Animations
{
    public interface IAuditableTransition : ITransition
    {
        event EventHandler Completed;
    }
}