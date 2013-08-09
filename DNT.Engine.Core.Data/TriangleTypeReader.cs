using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DNT.Engine.Core.Data
{
    public class TriangleTypeReader : ContentTypeReader<Triangle>
    {
        protected override Triangle Read(ContentReader input, Triangle existingInstance)
        {
            var p0 = input.ReadObject<Vector3>();
            var p1 = input.ReadObject<Vector3>();
            var p2 = input.ReadObject<Vector3>();
            var meshName = input.ReadString();
            return new Triangle(p0, p1, p2, meshName);
        }
    }
}
