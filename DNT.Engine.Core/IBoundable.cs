using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public interface IBoundable
    {
        BoundingSphere BoundingSphere { get; }
    }
}