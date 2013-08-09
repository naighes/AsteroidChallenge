using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Graphics
{
    public class SpriteAnimation
    {
        public SpriteAnimation(String name, SpriteAnimationFrame[] frames)
        {
            Verify.That(name).Named("name").IsNotNull();
            _name = name;

            if (frames.Length <= 0)
                throw new InvalidOperationException("At least one frame is needed.");

            _frames = frames;
        }

        protected Int32 CurrentFrameIndex
        {
            get { return _currentFrameIndex; }
            set { _currentFrameIndex = value; }
        }
        private Int32 _currentFrameIndex;

        protected Int32 FrameCount
        {
            get { return _frames.Length; }
        }

        public SpriteAnimationFrame CurrentFrame
        {
            get { return _frames[_currentFrameIndex]; }
        }

        private readonly SpriteAnimationFrame[] _frames;

        internal String Name
        {
            get { return _name; }
        }
        private readonly String _name;
        
        private Boolean _playing;

        public void Update(GameTime gameTime)
        {
            if (!_playing)
                return;

            if (CurrentFrame.Duration <= _elapsed)
            {
                _elapsed = TimeSpan.Zero;
                NextFrame();
            }
            else
                _elapsed += gameTime.ElapsedGameTime;
        }

        private TimeSpan _elapsed;

        public void Reset()
        {
            _currentFrameIndex = 0;
            _elapsed = TimeSpan.Zero;
        }

        public void Play()
        {
            _playing = true;
        }

        public void Stop()
        {
            _playing = false;
        }

        protected virtual void NextFrame()
        {
            var newIndex = CurrentFrameIndex + 1;
            CurrentFrameIndex = newIndex < FrameCount ? newIndex : CurrentFrameIndex;
        }
    }
}