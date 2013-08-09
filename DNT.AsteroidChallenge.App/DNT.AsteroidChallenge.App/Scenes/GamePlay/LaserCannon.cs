using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Graphics;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class LaserCannon : Weapon
    {
        public LaserCannon(Scene scene, TimeSpan chargeTime, Int32 munitions, IWorldObject source)
            : base(scene, chargeTime, munitions, source)
        {
        }

        protected override void Shot(IWorldObject source)
        {
            Scene.SoundManager.PlaySoundEffect("sounds/Bullet");
            var bullet = new LaserBullet(Scene,
                                         "textures/laser",
                                         source.World.Translation,
                                         Vector3.Normalize(source.World.Forward) * 1024.0f);
            var animation = new LoopedSpriteAnimation("Main", new[]
                                                                  {
                                                                      new SpriteAnimationFrame
                                                                          {
                                                                              Duration = TimeSpan.FromSeconds(0.1d),
                                                                              SourceRectangle = new Rectangle(0, 0, 256, 256)
                                                                          },
                                                                      new SpriteAnimationFrame
                                                                          {
                                                                              Duration = TimeSpan.FromSeconds(0.1d),
                                                                              SourceRectangle = new Rectangle(256, 0, 256, 256)
                                                                          },
                                                                      new SpriteAnimationFrame
                                                                          {
                                                                              Duration = TimeSpan.FromSeconds(0.1d),
                                                                              SourceRectangle = new Rectangle(512, 0, 256, 256)
                                                                          }
                                                                  });
            bullet.AddAnimation(animation)
                  .SetCurrentAnimation(animation)
                  .Play();
            Scene.AddComponent(bullet, "Models");
        }
    }
}