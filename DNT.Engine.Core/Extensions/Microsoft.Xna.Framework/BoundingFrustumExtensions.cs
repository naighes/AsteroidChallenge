using System;

namespace Microsoft.Xna.Framework
{
    public static class BoundingFrustumExtensions
    {
        public static Vector3 GenerateRandomPosition(this BoundingFrustum boundingFrustum,
                                                     Func<Single> func)
        {
            var corners = boundingFrustum.GetCorners();

            var np1 = Vector3.Lerp(corners[0], corners[1], func());
            var np2 = Vector3.Lerp(corners[2], corners[3], func());
            var np = Vector3.Lerp(np1, np2, func());

            var fp1 = Vector3.Lerp(corners[4], corners[5], func());
            var fp2 = Vector3.Lerp(corners[6], corners[7], func());
            var fp = Vector3.Lerp(fp1, fp2, func());

            return Vector3.Lerp(np, fp, func());
        }
    }
}
