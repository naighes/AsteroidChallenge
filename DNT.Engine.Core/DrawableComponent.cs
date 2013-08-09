using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core
{
    public abstract class DrawableComponent<T> : SceneComponent, IDrawableComponent where T : class
    {
        private Boolean _loaded;
        private Boolean _visible;

        protected DrawableComponent(Scene scene)
            : base(scene)
        {
            _visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            EnsureLoad();
        }

        protected virtual void OnBeginDraw()
        {
            Scene.GraphicsDevice.BlendState = BlendState.Opaque;
            Scene.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Scene.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Scene.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            EnsureLoad();

            if (!_visible)
                return;

            OnDrawing(EventArgs.Empty);

            OnBeginDraw();
            Render(view, projection);

            OnDrawn(EventArgs.Empty);
        }

        protected abstract void Render(Matrix view, Matrix projection);

        private void EnsureLoad()
        {
            if (_loaded)
                return;

            Load();
        }

        public virtual void Load()
        {
            _loaded = true;
        }

        public event EventHandler Drawing;

        private void OnDrawing(EventArgs e)
        {
            var handler = Drawing;

            if (handler.IsNotNull())
                handler(this, e);
        }

        public event EventHandler Drawn;

        public void Hide()
        {
            _visible = false;
        }

        public void Show()
        {
            _visible = true;
        }

        private void OnDrawn(EventArgs e)
        {
            var handler = Drawn;

            if (handler.IsNotNull())
                handler(this, e);
        }

        protected void SetEffectMatrices(IEffectMatrices effect, Matrix world) 
        {
            SetEffectMatrices(effect, Scene.CurrentCamera.View, Scene.CurrentCamera.Projection, world);
        }

        protected void SetEffectMatrices(IEffectMatrices effect, Matrix view, Matrix projection, Matrix world)
        {
            Verify.That(effect).Named("effect").IsNotNull();
            effect.View = view;
            effect.Projection = projection;
            effect.World = world;
        }

        protected abstract T Content { get; }
    }
}