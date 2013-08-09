using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Cameras;
using DNT.Engine.Core.Cameras.Builtin;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Graphics.Builtin;
using DNT.Engine.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Environment = DNT.Engine.Core.Context.Environment;

namespace DNT.AsteroidChallenge.App
{
    public class GamePlay : Scene
    {
        private readonly Random _random;
        private Ship _ship;
        private IEmitter _asteroidEmitter;
        private Emitter _randomStarsEmitter;
        private Sprite _gameOverSprite;
        private ICamera _camera;
        private IChasingCameraConfig _cameraConfig;

        public GamePlay(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider)
            : base(graphicsDevice, serviceProvider)
        {
            InputManager.StartAccelerometer();
            _random = new Random();
            Messenger.Register<ShipDestroyed>(this, OnShipDestroyed);
        }

        private ExplosionManager _explosionManager;

        private void OnShipDestroyed(Message<ShipDestroyed> message)
        {
            _explosionManager.AddExplosion(message.Content.Position, message.Content.Velocity);
            SoundManager.PlaySoundEffect("sounds/Explosion");

            _gameOverSprite.Show();
            _gameOverSprite.EnableClick();
            _asteroidEmitter.Stop();
            _randomStarsEmitter.Stop();
        }

        private readonly ShipInfo _shipInfo = new ShipInfo
        {
            DragFactor = 0.94f,
            EngineMaximumTorque = 40.0f,
            Mass = 1.0f,
            RotationRate = 1.9f,
            OriginalForward = Vector3.Forward,
            OriginalUp = Vector3.Up
        };

        private Boolean _hasBeenLoaded;

        protected override void Load()
        {
            if (_hasBeenLoaded)
                return;

            _hasBeenLoaded = true;

            Environment.Current().EnableDefaultLights().TurnLightsOn();

            InitializeGroups();
            InitializeParticleSystems();
            InitializeScorer();
            InitializeShip();

            InitializeCamera();
            AddComponent(_camera);
            SetCurrentCamera(_camera);

            InitializeSkyGlobe();
            InitializeCollisionMaps();
            InitializeAsteroidEmitter();
            InitializeRandomStarsEmitter();
            InitializeGameOverSprites();

            base.Load();
        }

        private void InitializeCollisionMaps()
        {
            AddCollisionMap(typeof(Asteroid), new[] { typeof(LaserBullet), typeof(Ship) });
        }

        private void InitializeSkyGlobe()
        {
            AddComponent(new SkyGlobe(this, "models/SkyGlobeTexture", 10.0f), "Environment");
        }

        private void InitializeGroups()
        {
            AddGroup("Environment");
            AddGroup("Models");
            AddGroup("Sprites");
        }

        private void InitializeScorer()
        {
            var scorer = new Scorer(this, "fonts/Segoe");
            AddComponent(scorer, "Sprites");
        }

        private void InitializeShip()
        {
            _ship = new Ship(this, "models/ships/feisar/FeisarShip", "models/SkyGlobeTexture", _shipInfo);
            _ship.SetMaterial(0, new Material { DiffuseColor = Vector3.One });
            _ship.AddWeapon(new LaserCannon(this, TimeSpan.FromSeconds(0.2d), -1, _ship), GestureType.Tap);
            AddComponent(_ship, "Models");
        }

        private void InitializeCamera()
        {
            _cameraConfig = Camera.AttachedTo(_ship)
                                  .At(new Vector3(0.0f, 0.0f, 200.0f))
                                  .Between(0.1f, 1000.0f)
                                  .Wide(MathHelper.PiOver4)
                                  .LookingAt(Vector3.Forward, new Vector3(0.0f, 25.0f, 0.0f))
                                  .KeepAwayOf(new Vector3(0.0f, 25.0f, 100.0f))
                                  .Weight(0.05f)
                                  .WithStiffnessOf(1.8f)
                                  .WithDampingOf(0.6f);
            _camera = new ChasingCamera(this, _cameraConfig);
        }

        private void InitializeAsteroidEmitter()
        {
            _asteroidEmitter = new AsteroidEmitter(this,
                                                   new[] { "models/Asteroid2" },
                                                   _ship,
                                                   _explosionManager)
                .EmitEvery(() => TimeSpan.FromSeconds(_random.Next(0.5f, 1.5f)))
                .ItemsPerTime(() => _random.Next(2, 8));
            AddComponent(_asteroidEmitter);
        }

        private void InitializeGameOverSprites()
        {
            _gameOverSprite = new Sprite(this, "textures/gameOver");
            _gameOverSprite.Hide();
            _gameOverSprite.DisableClick();
            _gameOverSprite.CenterHorizontally();
            _gameOverSprite.CenterVertically();
            AddComponent(_gameOverSprite, "Sprites");
            _gameOverSprite.Click += (sender, e) => SceneManager.PlayPreviousScene();
        }

        private void InitializeRandomStarsEmitter()
        {
            var randomStarsParticleSystem = new RandomStarsParticleSystem(this,
                                                                          BlendState.Additive,
                                                                          100,
                                                                          "textures/star");
            AddComponent(randomStarsParticleSystem, "Sprites");
            _randomStarsEmitter = new RandomStarsEmitter(this, randomStarsParticleSystem)
                .EmitEvery(() => TimeSpan.FromSeconds(_random.Next(0.5f, 1.5f)))
                .ItemsPerTime(() => _random.Next(4, 8));
            AddComponent(_randomStarsEmitter);
        }

        private void InitializeParticleSystems()
        {
            var fireParticleSystem = new ExplosionParticleSystem(this, BlendState.Additive, 100, "textures/explosion");
            fireParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));
            AddComponent(fireParticleSystem, "Sprites");

            var smokeParticleSystem = new ExplosionParticleSystem(this, BlendState.NonPremultiplied, 100, "textures/smoke");
            smokeParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));
            AddComponent(smokeParticleSystem, "Sprites");

            _explosionManager = new ExplosionManager(fireParticleSystem, smokeParticleSystem);
        }

        private void Restart()
        {
            // TODO: reset camera position.

            RemoveComponents<Asteroid>();

            _gameOverSprite.Hide();
            _gameOverSprite.DisableClick();

            _asteroidEmitter.Start();
            _randomStarsEmitter.Start();

            _ship.SetInitialStatus();
            InitializeCamera();
        }

        protected override void OnBeginPlay()
        {
            base.OnBeginPlay();

            Restart();
        }
    }
}