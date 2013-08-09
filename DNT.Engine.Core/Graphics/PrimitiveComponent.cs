using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public abstract class PrimitiveComponent<T> : DrawableComponent<Primitive<T>>, IDisposable
        where T : struct, IVertexType
    {
        protected PrimitiveComponent(Scene scene)
            : base(scene)
        {
            _content = new Primitive<T>();
        }

        protected void AddIndex(Int32 index)
        {
            _content.AddIndex(index);
        }

        protected void AddVertex(T vertex)
        {
            _content.AddVertex(vertex);
        }

        protected Int32 CurrentVertexCount
        {
            get { return _content.CurrentVertexCount; }
        }

        protected abstract void InitializeData();

        public override void Load()
        {
            base.Load();

            InitializeData();
            _content.InitializePrimitive(Scene.GraphicsDevice);
        }

        protected override Primitive<T> Content
        {
            get { return _content; }
        }
        private readonly Primitive<T> _content;

        protected override void Render(Matrix view, Matrix projection)
        {
            Scene.GraphicsDevice.SetVertexBuffer(_content.VertexBuffer);
            Scene.GraphicsDevice.Indices = _content.IndexBuffer;
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

            if (_content.IsNotNull())
                try { _content.Dispose(); }
                catch { }
        }

        ~PrimitiveComponent()
        {
            Dispose(false);
        }
    }
}