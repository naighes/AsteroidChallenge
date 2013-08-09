using System;
using DNT.Engine.Core.Data;
using Microsoft.Xna.Framework;
using Xunit;

namespace DNT.Engine.Core.TestFixtures
{
    public class TriangleExtensionsTests
    {
        [Fact]
        public void VertexInsideBoundingSphereCheck()
        {
            var transform = Matrix.CreateScale(3.0f)*Matrix.CreateRotationY(MathHelper.PiOver4);
            var sphere = new BoundingSphere(Vector3.Zero, 1.0f);
            var transformedSphere = sphere.Transform(transform);

            var triangleOut = new Triangle(new Vector3(2.1f, 2.1f, 0.0f),
                                           new Vector3(2.1f, 0.0f, 0.0f),
                                           new Vector3(4.2f, 2.1f, 0.0f),
                                           String.Empty);
            Assert.False(triangleOut.HasAtLeastOnPointInsideBoundingSphere(sphere));

            var transformedTriangleOut = new Triangle(Vector3.Transform(triangleOut.Point0, transform),
                                                      Vector3.Transform(triangleOut.Point1, transform),
                                                      Vector3.Transform(triangleOut.Point2, transform),
                                                      String.Empty);
            Assert.False(transformedTriangleOut.HasAtLeastOnPointInsideBoundingSphere(transformedSphere));


            var triangleIn = new Triangle(new Vector3(2.1f, 2.1f, 0.0f),
                                          new Vector3(0.9f, 0.0f, 0.0f), // It's in!
                                          new Vector3(4.2f, 2.1f, 0.0f),
                                          String.Empty);
            Assert.True(triangleIn.HasAtLeastOnPointInsideBoundingSphere(sphere));

            var transformedTriangleIn = new Triangle(Vector3.Transform(triangleIn.Point0, transform),
                                                     Vector3.Transform(triangleIn.Point1, transform),
                                                     Vector3.Transform(triangleIn.Point2, transform),
                                                     String.Empty);
            Assert.True(transformedTriangleIn.HasAtLeastOnPointInsideBoundingSphere(transformedSphere));
        }
    }
}