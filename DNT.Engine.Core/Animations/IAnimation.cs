using System;

namespace DNT.Engine.Core.Animations
{
    public interface IAnimation : IDrawableComponent
    {
        void Start();
        void Stop();
        void Start(Action endCallback);
        void Reset();
    }
}