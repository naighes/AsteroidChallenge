using System;

namespace DNT.Engine.Core.Physics
{
    public interface ISpring : ISolidBody
    {
        Single Stiffness { get; }
        Single DampingFactor { get; }
    }
}