using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Graphics
{
    public class AnimatedSprite : Sprite
    {
        public AnimatedSprite(Scene scene, String assetName)
            : base(scene, assetName)
        {
            _animations = new List<SpriteAnimation>();
        }

        public SpriteAnimation SetCurrentAnimation(String name)
        {
            Verify.That(name).Named("name").IsNotNullOrWhiteSpace();
            return SetCurrentAnimation(_animations.Single(a => a.Name == name));
        }

        public SpriteAnimation SetCurrentAnimation(SpriteAnimation animation)
        {
            Verify.That(animation).Named("animation").IsNotNull();
            return _currentAnimation = animation;
        }

        public AnimatedSprite AddAnimation(SpriteAnimation animation)
        {
            Verify.That(animation).Named("animation").IsNotNull();
            _animations.Add(animation);
            return this;
        }

        private readonly IList<SpriteAnimation> _animations;

        protected override Rectangle SourceRectangle
        {
            get
            {
                return CurrentAnimation.IsNull() ||
                       !CurrentAnimation.CurrentFrame.SourceRectangle.HasValue
                           ? base.SourceRectangle
                           : CurrentAnimation.CurrentFrame.SourceRectangle.Value;
            }
        }

        protected SpriteAnimation CurrentAnimation
        {
            get { return _currentAnimation; }
        }
        private SpriteAnimation _currentAnimation;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentAnimation.IsNull())
                return;

            CurrentAnimation.Update(gameTime);
        }
    }
}