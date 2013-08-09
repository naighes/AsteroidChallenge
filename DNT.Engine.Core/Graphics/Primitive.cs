using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public class Primitive<T> : IDisposable where T : struct, IVertexType
    {
        public Primitive()
        {
            _vertices = new List<T>();
            _indices = new List<UInt16>();
        }

        protected internal void AddIndex(Int32 index)
        {
            if (index > UInt16.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            _indices.Add((UInt16)index);
        }

        protected internal void AddVertex(T vertex)
        {
            _vertices.Add(vertex);
        }

        protected internal Int32 CurrentVertexCount
        {
            get { return _vertices.Count; }
        }

        protected IList<T> Vertices
        {
            get { return _vertices; }
        }
        private readonly IList<T> _vertices;

        protected IList<UInt16> Indices
        {
            get { return _indices; }
        }

        protected internal IndexBuffer IndexBuffer
        {
            get { return _indexBuffer; }
        }

        protected internal VertexBuffer VertexBuffer
        {
            get { return _vertexBuffer; }
        }

        private readonly IList<UInt16> _indices;

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        protected internal void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
            _vertexBuffer = new VertexBuffer(graphicsDevice,
                                             typeof(T),
                                             _vertices.Count,
                                             BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());
            _indexBuffer = new IndexBuffer(graphicsDevice, typeof(UInt16), _indices.Count, BufferUsage.None);
            _indexBuffer.SetData(_indices.ToArray());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(Boolean disposing)
        {
            if (!disposing)
                return;

            if (_vertexBuffer.IsNotNull())
                try { _vertexBuffer.Dispose(); }
                catch { }

            if (_indexBuffer.IsNotNull())
                try { _indexBuffer.Dispose(); }
                catch { }
        }

        ~Primitive()
        {
            Dispose(false);
        }
    }
}