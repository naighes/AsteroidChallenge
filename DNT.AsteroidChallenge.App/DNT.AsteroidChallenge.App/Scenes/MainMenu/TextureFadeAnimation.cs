using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Animations;
using DNT.Engine.Core.Graphics;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class TextureFadeAnimation : Sprite, IAnimation
    {
        private readonly IAuditableTransition _transition;
        private Rectangle _viewPortArea;

        public TextureFadeAnimation(Scene scene,
                                    String assetName,
                                    IAuditableTransition transition)
            : base(scene, assetName)
        {
            _transition = transition;
            _transition.Completed += OnCompleted;
        }

        public void Start()
        {
            Start(null);
        }

        public void Start(Action endCallback)
        {
            _viewPortArea = Scene.Viewport.TitleSafeArea;
            _isPlaying = true;
            _endCallback = endCallback;
        }

        private void OnCompleted(Object sender, EventArgs e)
        {
            if (_endCallback.IsNotNull())
                _endCallback();
        }

        private Action _endCallback;

        public void Stop()
        {
            _isPlaying = false;
        }

        private Boolean _isPlaying;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetDrawColor(Color.Black * _transition.CurrentTransitionAmount);

            if (_isPlaying)
                _transition.Update(gameTime);
        }

        protected override Rectangle DestinationRectangle
        {
            get { return new Rectangle(0, 0, _viewPortArea.Width, _viewPortArea.Height); }
        }

        public void Reset()
        {
            Stop();
            _transition.Reset();
        }
    }
}