using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Animations;
using DNT.Engine.Core.Cameras;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Graphics.Builtin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.AsteroidChallenge.App
{
    public class MainMenu : Scene
    {
        public MainMenu(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider)
            : base(graphicsDevice, serviceProvider)
        {
        }

        private TextureFadeAnimation _fadeOutAnimation;
        private TextureFadeAnimation _fadeInAnimation;

        private Sprite _startGameMenuItem;

        private Boolean _hasBeenLoaded;

        protected override void Load()
        {
            if (_hasBeenLoaded)
                return;

            _hasBeenLoaded = true;

            AddGroup("Environment");
            AddGroup("Models");
            AddGroup("Sprites");

            var camera = new MainMenuCamera(this, Camera.Default().Between(0.1f, 1000.0f).Wide(MathHelper.PiOver4));
            AddComponent(camera);
            SetCurrentCamera(camera);

            AddComponent(new SkyGlobe(this, "models/SkyGlobeTexture", 200.0f), "Environment");
            AddComponent(new Sprite(this, "textures/logo").FillScreen(), "Sprites");

            _startGameMenuItem = new Sprite(this, "textures/startGame");
            AddComponent(_startGameMenuItem, "Sprites");

            _fadeOutAnimation = new TextureFadeAnimation(this,
                                                         "textures/blank",
                                                         BuildTransition.Between(1.0f, 0.0f)
                                                                        .Within(TimeSpan.FromSeconds(0.5f))
                                                                        .InterpolateWith(MathHelper.Lerp)
                                                                        .Auditable()
                                                                        .Instance());
            AddComponent(_fadeOutAnimation, "Sprites");

            _fadeInAnimation = new TextureFadeAnimation(this,
                                                        "textures/blank",
                                                        BuildTransition.Between(0.0f, 1.0f)
                                                                       .Within(TimeSpan.FromSeconds(0.5f))
                                                                       .InterpolateWith(MathHelper.SmoothStep)
                                                                       .Auditable()
                                                                       .Instance());
            AddComponent(_fadeInAnimation, "Sprites");
            _startGameMenuItem.Click += (sender, e) =>
                                          {
                                              _startGameMenuItem.DisableClick();
                                              _fadeInAnimation.Start(SceneManager.PlayNextScene);
                                          };

            _startGameMenuItem.DisableClick()
                              .SetPosition(new Vector2(0.0f, 340.0f))
                              .CenterHorizontally();

            base.Load();
        }

        protected override void OnBeginPlay()
        {
            base.OnBeginPlay();

            _fadeOutAnimation.Reset();
            _fadeInAnimation.Reset();
            _fadeOutAnimation.Start(() => _startGameMenuItem.EnableClick());
        }
    }
}