using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras
{
    public interface ICamera : ISceneComponent
    {
        Matrix View { get; }
        Matrix Projection { get; }
        Vector3 Position { get; }
        BoundingFrustum BoundingFrustum { get; }
        Vector3 Target { get; }
        Vector3 Right { get; }
        Vector3 Up { get; }
        Vector3 Forward { get; }
        Matrix ViewProjection { get; }
        Matrix InvertedViewProjection { get; }
    }
}