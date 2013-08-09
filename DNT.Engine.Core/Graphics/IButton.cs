using System;

namespace DNT.Engine.Core.Graphics
{
    public interface IButton
    {
        event EventHandler<SpriteEventArgs> Click;
    }
}