using System;

namespace DNT.Engine.Core.Graphics
{
    public class LoopedSpriteAnimation : SpriteAnimation
    {
        public LoopedSpriteAnimation(String name, SpriteAnimationFrame[] frames)
            : base(name, frames)
        {
        }

        protected override void NextFrame()
        {
            var newIndex = CurrentFrameIndex + 1;
            CurrentFrameIndex = newIndex < FrameCount ? newIndex : 0;
        }
    }
}