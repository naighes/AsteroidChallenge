using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics.Utils
{
    public static class BoundingSphereRenderer
    {
        private const Int32 SphereResolution = 30;
        private static readonly IDictionary<Guid, RendererHelperData> Subscriptions = new Dictionary<Guid, RendererHelperData>();

        public static void InitializeGraphics(GraphicsDevice graphicsDevice,
                                              Guid id)
        {
            var basicEffect = new BasicEffect(graphicsDevice)
                                  {
                                      LightingEnabled = false,
                                      VertexColorEnabled = false
                                  };

            var vertices = new VertexPositionColor[(SphereResolution + 1) * 3];
            var index = 0;
            var step = MathHelper.TwoPi / SphereResolution;

            for (var a = 0.0f; a <= MathHelper.TwoPi; a += step)
                vertices[index++] =
                    new VertexPositionColor(new Vector3((Single) Math.Cos(a), (Single) Math.Sin(a), 0.0f), Color.White);

            for (var a = 0f; a <= MathHelper.TwoPi; a += step)
                vertices[index++] =
                    new VertexPositionColor(new Vector3((Single) Math.Cos(a), 0.0f, (Single) Math.Sin(a)), Color.White);

            for (var a = 0f; a <= MathHelper.TwoPi; a += step)
                vertices[index++] =
                    new VertexPositionColor(new Vector3(0.0f, (Single) Math.Cos(a), (Single) Math.Sin(a)), Color.White);

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

        public static void Render(BoundingSphere sphere,
                                  GraphicsDevice graphicsDevice,
                                  Matrix view,
                                  Matrix projection,
                                  Color color,
                                  Guid id)
        {
            var subscription = Subscriptions[id];

            graphicsDevice.SetVertexBuffer(subscription.VertexBuffer);
            subscription.BasicEffect.World = Matrix.CreateScale(sphere.Radius)*
                                                   Matrix.CreateTranslation(sphere.Center);
            subscription.BasicEffect.View = view;
            subscription.BasicEffect.Projection = projection;
            subscription.BasicEffect.DiffuseColor = color.ToVector3();

            foreach (var pass in subscription.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, SphereResolution);
                graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip,
                                              SphereResolution + 1,
                                              SphereResolution);
                graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip,
                                              (SphereResolution + 1)*2,
                                              SphereResolution);
            }
        }
    }
}