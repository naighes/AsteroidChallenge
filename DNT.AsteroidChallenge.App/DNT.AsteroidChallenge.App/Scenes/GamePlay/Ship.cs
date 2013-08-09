using System;
using System.Collections.Generic;
using DNT.Engine.Core;
using DNT.Engine.Core.CollisionsDetection;
using DNT.Engine.Core.Data;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Messaging;
using DNT.Engine.Core.Physics;
using DNT.Engine.Core.Validation;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Environment = DNT.Engine.Core.Context.Environment;

namespace DNT.AsteroidChallenge.App
{
    public struct ShipDestroyed
    {
        public ShipDestroyed(Vector3 position, Vector3 velocity)
        {
            _position = position;
            _velocity = velocity;
        }

        public Vector3 Position
        {
            get { return _position; }
        }
        private readonly Vector3 _position;

        public Vector3 Velocity
        {
            get { return _velocity; }
        }
        private readonly Vector3 _velocity;
    }

    public class Ship : SceneModel, ISolidBody, ICollidable
    {
        public Ship(Scene scene,
                    String assetName,
                    String environmentTextureAssetName,
                    ShipInfo shipInfo)
            : base(scene, assetName)
        {
            Verify.That(environmentTextureAssetName).Named("environmentTextureAssetName").IsNotNullOrWhiteSpace();
            _environmentTextureAssetName = environmentTextureAssetName;

            _shipInfo = shipInfo;
            _weapons = new Dictionary<Weapon, GestureType>();
            Scene.InputManager.SubscribeAccelerometer(this, OnAccelerometerReading);
            _originalForward = shipInfo.OriginalForward;
        }

        private readonly ShipInfo _shipInfo;

        private Vector3 _forward;
        private readonly Vector3 _originalForward;

        private Vector3 _position;
        private Vector3 _velocity;

        private Vector2 _inputRotation;
        private Quaternion _rotation;

        private EnvironmentMapEffect _effect;
        
        public override Matrix World
        {
            get { return _world; }
        }
        private Matrix _world;

        public void SetInitialStatus()
        {
            _destroyed = false;
            _forward = _shipInfo.OriginalForward;
            _world = Matrix.Identity;
            _rotation = Quaternion.Identity;
            _thrustAmount = 5.0f;
            _position = Vector3.Zero;
            EnableWeapons();
            Show();
        }

        public void CheckCollision(ICollidable target) { }

        public Single DragFactor
        {
            get { return _shipInfo.DragFactor; }
        }

        public Single Mass
        {
            get { return _shipInfo.Mass; }
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        private void SetForward(Vector3 forward)
        {
            _forward = Vector3.Normalize(forward);
        }

        private void OnAccelerometerReading(Message<AccelerometerReading> message)
        {
            if (_destroyed)
                return;

            var rotationX = -message.Content.Acceleration.Y;
            var rotationY = (message.Content.Acceleration.Z + 0.8f);
            _inputRotation = new Vector2(-rotationX, rotationY) * 2.5f;
        }

        public override void Load()
        {
            base.Load();

#if DEBUG
            Engine.Core.Graphics.Utils.BoundingSphereRenderer.InitializeGraphics(Scene.GraphicsDevice, Id);
            Engine.Core.Graphics.Utils.TrianglesRenderer.InitializeGraphics(Scene.GraphicsDevice, Triangles, Id);
#endif

            _effect = new EnvironmentMapEffect(Scene.GraphicsDevice);
            _environment = Scene.Content.Load<TextureCube>(_environmentTextureAssetName);
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateRotation(gameTime);
            UpdatePosition(gameTime);

            _world = Matrix.Identity *
                     Matrix.CreateFromQuaternion(_rotation) *
                     Matrix.CreateTranslation(_position);
        }

        private void UpdateRotation(GameTime gameTime)
        {
            var rotationAmount = _inputRotation;

            if (rotationAmount.X > -0.1f && rotationAmount.X < 0.1f)
                rotationAmount.X = 0.0f;

            if (rotationAmount.Y > -0.1f && rotationAmount.Y < 0.1f)
                rotationAmount.Y = 0.0f;

            rotationAmount = rotationAmount * _shipInfo.RotationRate * (Single)gameTime.ElapsedGameTime.TotalSeconds;

            _rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, rotationAmount.X) *
                         Quaternion.CreateFromAxisAngle(Vector3.Right, rotationAmount.Y);

            SetForward(Vector3.Transform(_originalForward, _rotation));
        }

        private Single _thrustAmount;

        private void UpdatePosition(GameTime gameTime)
        {
            var delta = (Single)gameTime.ElapsedGameTime.TotalSeconds;

            var force = _forward * _thrustAmount * _shipInfo.EngineMaximumTorque;
            var acceleration = force / Mass;
            _velocity += acceleration * delta;
            _velocity *= DragFactor;
            _position += _velocity * delta;
        }

        #endregion

        #region Drawing

        protected override void SetupEffect(Effect effect, Matrix view, Matrix projection, Matrix world)
        {
            base.SetupEffect(effect, view, projection, world);

            var e = effect as EnvironmentMapEffect;

            if (e.IsNull())
                return;

            Environment.Current().SetLights(e);
            e.EnvironmentMap = _environment;

            // TODO: Make material info at "type" level.
            e.EnvironmentMapAmount = 1.0f;
            e.FresnelFactor = 1.0f;
            e.EnvironmentMapSpecular = Vector3.Zero;
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            base.Render(view, projection);

#if DEBUG
            Engine.Core.Graphics.Utils.BoundingSphereRenderer.Render(BoundingSphere,
                                                                     Scene.GraphicsDevice,
                                                                     view,
                                                                     projection,
                                                                     Color.Green,
                                                                     Id);

            foreach (var mesh in Content.Meshes)
            {
                var triangles = mesh.Tag as Triangle[];
                Engine.Core.Graphics.Utils.TrianglesRenderer.Render(Scene.GraphicsDevice,
                                                                    view,
                                                                    projection,
                                                                    GetWorldForMesh(mesh),
                                                                    Color.Green,
                                                                    triangles,
                                                                    Id);
            }
#endif

            Scene.GraphicsDevice.ResetRenderStates();
        }

        protected override Effect GetCustomEffect()
        {
            return _effect;
        }

        #endregion

        #region Weapons

        public void AddWeapon(Weapon weapon, GestureType gesture)
        {
            Verify.That(weapon).Named("weapon").IsNotNull();

            if (_weapons.ContainsKey(weapon))
                throw new IndexOutOfRangeException("Weapon list already contains specified element.");

            _weapons.Add(weapon, gesture);
            Scene.InputManager.SubscribeTap(this, message => weapon.Shot());

            Scene.AddComponent(weapon);
        }

        public void RemoveWeapon(Weapon weapon)
        {
            Verify.That(weapon).Named("weapon").IsNotNull();

            if (!_weapons.ContainsKey(weapon))
                throw new IndexOutOfRangeException("Weapon list doesn't contain specified element.");

            // TODO
            //Scene.InputManager.UnsubscribeAllGestures(this, _weapons[weapon]);
            _weapons.Remove(weapon);
            Scene.RemoveComponent(weapon);
        }

        private readonly IDictionary<Weapon, GestureType> _weapons;

        #endregion

        private TextureCube _environment;
        private readonly String _environmentTextureAssetName;
        private Boolean _destroyed;

        public void Destroy()
        {
            if (_destroyed)
                return;

            _destroyed = true;
            Messenger.Send(new Message<ShipDestroyed>(new ShipDestroyed(_position, _velocity)));

            Hide();
            _thrustAmount = 0.0f;
            _inputRotation = Vector2.Zero;
            DisableWeapons();
        }

        private void DisableWeapons()
        {
            foreach (var weapon in _weapons)
                weapon.Key.Disable();
        }

        private void EnableWeapons()
        {
            foreach (var weapon in _weapons)
                weapon.Key.Enable();
        }
    }
}