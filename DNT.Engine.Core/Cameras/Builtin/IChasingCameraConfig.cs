using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras.Builtin
{
    public interface IChasingCameraConfig : ICameraConfig
    {
        Vector3 PositionOffset { get; }
        Vector3 LookAtOffset { get; }
        IWorldObject ChasedObject { get; }
        Single Stiffness { get; }
        Single DampingFactor { get; }
        Single Mass { get; }
        Single DragFactor { get; }
    }
}