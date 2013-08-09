using System;

namespace DNT.Engine.Core.Physics
{
    public interface ISolidBody
    {
        Single Mass { get; }
        Single DragFactor { get; }
    }
}