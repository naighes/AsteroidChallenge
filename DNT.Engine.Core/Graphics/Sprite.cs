using System;
using DNT.Engine.Core.Messaging;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Graphics
{
    public class Sprite : DrawableComponent<Texture2D>, IButton
    {
        public Sprite(Scene scene, String assetName)
            : this(scene, assetName, Rectangle.Empty)
        {
        }

        public Sprite(Scene scene, String assetName, Rectangle sourceRectangle)
            : base(scene)
        {
            Verify.That(assetName).Named("assetName").IsNotNullOrWhiteSpace();
            _assetName = assetName;

            _blendState = BlendState.AlphaBlend;
            _samplerState = SamplerState.LinearClamp;
            _depthStencilState = DepthStencilState.None;
            _rasterizerState = RasterizerState.CullNone;
            _color = Color.White;
            _sourceRectangle = sourceRectangle;
        }

        private readonly String _assetName;
        private Boolean _shouldBeCenteredHorizontally;
        private Boolean _shouldBeCenteredVertically;

        protected virtual Rectangle SourceRectangle
        {
            get
            {
                if (_sourceRectangle.IsNotEmpty())
                    return _sourceRectangle;

                return Content.IsNull()
                           ? Rectangle.Empty
                           : new Rectangle(0, 0, Content.Width, Content.Height);
            }
        }
        private Rectangle _sourceRectangle;

        protected virtual Rectangle DestinationRectangle
        {
            get
            {
                if (_destinationRectangle.IsNotEmpty())
                    return _destinationRectangle;

                if (SourceRectangle.IsNotEmpty())
                    return new Rectangle((Int32)_position.X,
                                         (Int32)_position.Y,
                                         SourceRectangle.Width,
                                         SourceRectangle.Height);

                return Content.IsNull()
                           ? Rectangle.Empty
                           : new Rectangle((Int32)_position.X, (Int32)_position.Y, Content.Width, Content.Height);
            }
        }
        private Rectangle _destinationRectangle;

        public Sprite SetDestinationRectangle(Rectangle rectangle)
        {
            _destinationRectangle = rectangle;
            return this;
        }

        public Sprite SetPosition(Vector2 position)
        {
            _position = position;
            return this;
        }

        private Vector2 _position;

        public Sprite CenterHorizontally()
        {
            if (_destinationRectangle.IsNotEmpty() || Content.IsNotNull())
                _position.X = DestinationRectangle.GetHorizontalCenter(Scene.Viewport.TitleSafeArea);
            else
                _shouldBeCenteredHorizontally = true;

            return this;
        }

        public Sprite CenterVertically()
        {
            if (_destinationRectangle.IsNotEmpty() || Content.IsNotNull())
                _position.Y = DestinationRectangle.GetVerticalCenter(Scene.Viewport.TitleSafeArea);
            else
                _shouldBeCenteredVertically = true;

            return this;
        }

        public Sprite FillScreen()
        {
            _destinationRectangle = Scene.Viewport.TitleSafeArea;
            return this;
        }

        protected override Texture2D Content
        {
            get { return _content; }
        }
        private Texture2D _content;

        public override void Load()
        {
            base.Load();

            _content = Scene.Content.Load<Texture2D>(_assetName);

            if (Content.IsNull())
                throw new InvalidOperationException(String.Format("Cannot find a texture for asset name '{0}'.",
                                                                  _assetName));

            if (_shouldBeCenteredHorizontally)
                _position.X = DestinationRectangle.GetHorizontalCenter(Scene.Viewport.TitleSafeArea);

            if (_shouldBeCenteredVertically)
                _position.Y = DestinationRectangle.GetVerticalCenter(Scene.Viewport.TitleSafeArea);
        }

        public Sprite SetBlendState(BlendState blendState)
        {
            _blendState = blendState.IsNull() ? BlendState.AlphaBlend : blendState;
            return this;
        }
        private BlendState _blendState;

        public Sprite SetSamplerState(SamplerState samplerState)
        {
            _samplerState = samplerState.IsNull() ? SamplerState.LinearClamp : samplerState;
            return this;
        }
        private SamplerState _samplerState;

        public Sprite SetDepthStencilState(DepthStencilState depthStencilState)
        {
            _depthStencilState = depthStencilState.IsNull() ? DepthStencilState.None : depthStencilState;
            return this;
        }
        private DepthStencilState _depthStencilState;

        public Sprite SetRasterizerState(RasterizerState rasterizerState)
        {
            _rasterizerState = rasterizerState.IsNull() ? RasterizerState.CullNone : rasterizerState;
            return this;
        }
        private RasterizerState _rasterizerState;

        public Sprite UseCustomEffect(Effect effect)
        {
            _effect = effect;
            return this;
        }
        private Effect _effect;

        public Sprite SetDrawColor(Color color)
        {
            _color = color;
            return this;
        }
        private Color _color;

        public Sprite SetRotation(Single rotation)
        {
            _rotation = rotation;
            return this;
        }
        private Single _rotation;

        public Sprite SetLayerDepth(Single layerDepth)
        {
            _layerDepth = layerDepth;
            return this;
        }
        private Single _layerDepth;

        public Sprite SetOrigin(Vector2 origin)
        {
            _origin = origin;
            return this;
        }
        private Vector2 _origin;

        protected override void Render(Matrix view, Matrix projection)
        {
            Scene.SpriteBatch.Begin(SpriteSortMode.Immediate,
                                    _blendState,
                                    _samplerState,
                                    _depthStencilState,
                                    _rasterizerState,
                                    _effect);
            Scene.SpriteBatch.Draw(Content,
                                   DestinationRectangle,
                                   SourceRectangle,
                                   _color,
                                   _rotation,
                                   _origin,
                                   SpriteEffects.None,
                                   _layerDepth);
            Scene.SpriteBatch.End();
            Scene.GraphicsDevice.ResetRenderStates();
        }

        #region IButton

        private Boolean _disabled;

        public event EventHandler<SpriteEventArgs> Click
        {
            add
            {
                Scene.InputManager.SubscribeTap(this, OnTap);
                _click += value;
            }
            remove { _click -= value; }
        }
        private EventHandler<SpriteEventArgs> _click;

        private void OnTap(Message<TapGesture> message)
        {
            if (message.Content.GestureSample.Position.IsHover(DestinationRectangle) &&
                !_disabled)
                OnClick(message.Content.GestureSample.Position);
        }

        private void OnClick(Vector2 clickCoordinates)
        {
            OnClick(new SpriteEventArgs(clickCoordinates));
        }

        private void OnClick(SpriteEventArgs args)
        {
            if (_click.IsNull())
                return;

            _click(this, args);
        }

        public Sprite DisableClick()
        {
            _disabled = true;
            return this;
        }

        public Sprite EnableClick()
        {
            _disabled = false;
            return this;
        }

        #endregion
    }
}