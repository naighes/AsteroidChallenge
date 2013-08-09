using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Data
{
    public static class TriangleExtensions
    {
        public static Plane ToPlane(this Triangle triangle)
        {
            return new Plane(triangle.Point0, triangle.Point1, triangle.Point2);
        }

        public static Boolean HasAtLeastOnPointInsideBoundingSphere(this Triangle triangle, BoundingSphere boundingSphere)
        {
            return HasAtLeastOnPointInsideBoundingSphere(triangle, boundingSphere, Matrix.Identity);
        }

        public static Boolean HasAtLeastOnPointInsideBoundingSphere(this Triangle triangle,
                                                                    BoundingSphere boundingSphere,
                                                                    Matrix transformation)
        {
            foreach (var point in triangle.Points)
            {
                // Point must be tranformed.
                var transformedPoint = Vector3.Transform(point, transformation);

                if (transformedPoint.IsInsideBoundingSphere(boundingSphere))
                    return true;
            }

            return false;
        }
    }
}