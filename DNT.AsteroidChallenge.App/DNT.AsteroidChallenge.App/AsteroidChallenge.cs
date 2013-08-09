using DNT.Engine.Core;
using DNT.Engine.Core.Audio;
using Microsoft.Xna.Framework.Input.Touch;

namespace DNT.AsteroidChallenge.App
{
    public class AsteroidChallenge : GameBase
    {
        protected override void Initialize()
        {
            SceneManager.AddScenes(new MainMenu(GraphicsDevice, Services),
                                   new GamePlay(GraphicsDevice, Services));

            ((SoundManager)Services.GetService(typeof(SoundManager))).AddSoundEffect("sounds/Bullet");
            ((SoundManager)Services.GetService(typeof(SoundManager))).AddSoundEffect("sounds/Explosion");

            base.Initialize();
        }

        public AsteroidChallenge()
        {
            // Enable a subset of gestures.
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag;
        }
    }
}