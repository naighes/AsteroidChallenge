using System;

namespace Microsoft.Xna.Framework
{
    public static class Vector3Extensions
    {
        public static Boolean IsInsideBoundingSphere(this Vector3 point, BoundingSphere boundingSphere)
        {
            var diffVect = point - boundingSphere.Center;
            return diffVect.LengthSquared() < Math.Abs(boundingSphere.Radius * boundingSphere.Radius);
        }
    }
}