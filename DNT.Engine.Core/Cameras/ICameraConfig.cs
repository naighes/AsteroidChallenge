using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras
{
    public interface ICameraConfig
    {
        Single NearPlaneDistance { get; }
        Single FarPlaneDistance { get; }
        Single FieldOfView { get; }
        Vector3 Position { get; }
        Vector3 Up { get; }
        Vector3 Forward { get; }
    }
}