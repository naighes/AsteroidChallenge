using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics.Utils
{
    public static class BoundingFrustumRenderer
    {
        private static readonly VertexPositionColor[] Vertices = new VertexPositionColor[8];

        private static readonly Int16[] Indices = new Int16[]
        {
            0, 1, 1,
            2, 2, 3,
            3, 0, 0,
            4, 1, 5,
            2, 6, 3,
            7, 4, 5,
            5, 6, 6,
            7, 7, 4
        };

        private static BasicEffect _basicEffect;
        
        public static void Render(BoundingFrustum frustum,
                                  GraphicsDevice graphicsDevice,
                                  Matrix view,
                                  Matrix projection,
                                  Color color)
        {
            if (_basicEffect.IsNull())
                _basicEffect = new BasicEffect(graphicsDevice)
                                   {
                                       VertexColorEnabled = true,
                                       LightingEnabled = false
                                   };

            var corners = frustum.GetCorners();

            for (var i = 0; i < 8; i++)
            {
                Vertices[i].Position = corners[i];
                Vertices[i].Color = color;
            }

            _basicEffect.View = view;
            _basicEffect.Projection = projection;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                                         Vertices,
                                                         0,
                                                         8,
                                                         Indices,
                                                         0,
                                                         Indices.Length/2);
            }
        }
    }
}