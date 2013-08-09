using System;
using DNT.Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras.Builtin
{
    public class ChasingCamera : CameraBase, ISpring
    {
        public ChasingCamera(Scene scene, IChasingCameraConfig config)
            : base(scene, config)
        {
            _chasedObject = config.ChasedObject;
            _positionOffset = config.PositionOffset;
            _lookAtOffset = config.LookAtOffset;
            _stiffness = config.Stiffness;
            _dampingFactor = config.DampingFactor;
            _mass = config.Mass;
            SetPosition(config.Position);
            SetUp(config.Up);
            UpdateRotation();
        }

        private readonly IWorldObject _chasedObject;
        private readonly Vector3 _positionOffset;
        private readonly Vector3 _lookAtOffset;

        public Single Mass
        {
            get { return _mass; }
        }
        private readonly Single _mass;

        public Single DragFactor
        {
            get { throw new NotImplementedException(); }
        }

        public Single Stiffness
        {
            get { return _stiffness; }
        }
        private readonly Single _stiffness;

        public Single DampingFactor
        {
            get { return _dampingFactor; }
        }
        private readonly Single _dampingFactor;

        private Vector3 _velocity;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var elapsed = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            var rotation = UpdateRotation();

            var stretch = Position - (_chasedObject.World.Translation + Vector3.Transform(_positionOffset, rotation));
            var force = -_stiffness * stretch - _dampingFactor * _velocity;

            var acceleration = force / _mass;
            _velocity += acceleration * elapsed; // a * t

            SetPosition(Position + _velocity * elapsed); // a * t * t
        }

        private Quaternion UpdateRotation()
        {
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            _chasedObject.World.Decompose(out scale, out rotation, out translation);

            SetTarget(_chasedObject.World.Translation + Vector3.Transform(_lookAtOffset, rotation));
            SetUp(_chasedObject.World.Up);
            return rotation;
        }
    }
}