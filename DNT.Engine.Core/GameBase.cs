using System;
using DNT.Engine.Core.Audio;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DNT.Engine.Core
{
    public abstract class GameBase : Game
    {
        protected GameBase()
        {
            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            IsFixedTimeStep = false;

            _graphicsDeviceManager = new GraphicsDeviceManager(this) { IsFullScreen = true };
            _graphicsDeviceManager.SetLandscapeMode();

            Services.AddService(typeof(IInputManager), new InputManager());
            Services.AddService(typeof(ContentManager), Content);
            Services.AddService(typeof(SceneManager), new SceneManager());
            Services.AddService(typeof(SoundManager), new SoundManager(Content));
            Services.AddService(typeof(IEffectConfigurer), new EffectConfigurer());
        }

        public SceneManager SceneManager
        {
            get { return Services.GetService(typeof(SceneManager)) as SceneManager; }
        }

        protected override void Initialize()
        {
            SceneManager.Initialize();

            base.Initialize();
        }

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        protected override void LoadContent()
        {
            base.LoadContent();

            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
            SceneManager.PlayCurrentScene();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            SceneManager.Draw();
        }
    }
}