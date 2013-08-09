using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics.Utils
{
    public static class TrianglesRenderer
    {
        private static readonly IDictionary<Guid, RendererHelperData> Subscriptions = new Dictionary<Guid, RendererHelperData>();

        public static void InitializeGraphics(GraphicsDevice graphicsDevice,
                                              Triangle[] triangles,
                                              Guid id)
        {
            var basicEffect = new BasicEffect(graphicsDevice)
                                  {
                                      LightingEnabled = false,
                                      VertexColorEnabled = false
                                  };

            var index = 0;
            var vertices = new VertexPositionColor[triangles.SelectMany(i => i.Points).Count()];

            foreach (var point in triangles.SelectMany(triangle => triangle.Points))
                vertices[index++] = new VertexPositionColor(new Vector3(point.X,
                                                                        point.Y,
                                                                        point.Z),
                                                            Color.White);

            var vertexBuffer = new VertexBuffer(graphicsDevice,
                                                typeof (VertexPositionColor),
                                                vertices.Length,
                                                BufferUsage.None);
            vertexBuffer.SetData(vertices);

            Subscriptions.Add(id, new RendererHelperData
                                       {
                                           BasicEffect = basicEffect,
                                           VertexBuffer = vertexBuffer
                                       });
        }

        public static void Render(GraphicsDevice graphicsDevice,
                                  Matrix view,
                                  Matrix projection,
                                  Matrix world,
                                  Color color,
                                  Triangle[] triangles,
                                  Guid id)
        {
            var subscription = Subscriptions[id];

            graphicsDevice.SetVertexBuffer(subscription.VertexBuffer);
            subscription.BasicEffect.World = world;
            subscription.BasicEffect.View = view;
            subscription.BasicEffect.Projection = projection;
            subscription.BasicEffect.DiffuseColor = color.ToVector3();

            foreach (var pass in subscription.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, triangles.Length * 3);
            }
        }
    }
}