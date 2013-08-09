using System.Linq;

namespace Microsoft.Xna.Framework
{
    public static class BoundingSphereExtensions
    {
        public static BoundingSphere Transform(this BoundingSphere source, Matrix transformation)
        {
            Vector3 translation;
            Vector3 scaling;
            Quaternion rotation;
            transformation.Decompose(out scaling, out rotation, out translation);

            var transformedSphereRadius = source.Radius * new[] { scaling.X, scaling.Y, scaling.Z }.Max();
            var transformedSphereCenter = Vector3.Transform(source.Center, transformation);

            return new BoundingSphere(transformedSphereCenter, transformedSphereRadius);
        }
    }
}