using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras.Builtin
{
    public class ChasingCameraConfig : IChasingCameraConfig
    {
        private readonly CameraConfig _cameraConfig;
        private readonly IWorldObject _chasedObject;
        private Vector3 _positionOffset;
        private Vector3 _lookAtOffset;

        internal ChasingCameraConfig(IWorldObject chasedObject)
        {
            _cameraConfig = new CameraConfig();
            _chasedObject = chasedObject;
            _positionOffset = Vector3.Backward;
            _lookAtOffset = Vector3.Zero;
        }

        public ChasingCameraConfig Between(Single nearPlaneDistance, Single farPlaneDistance)
        {
            _cameraConfig.Between(nearPlaneDistance, farPlaneDistance);
            return this;
        }

        public ChasingCameraConfig Wide(Single fieldOfView)
        {
            _cameraConfig.Wide(fieldOfView);
            return this;
        }

        public ChasingCameraConfig At(Vector3 position)
        {
            _cameraConfig.At(position);
            return this;
        }

        public ChasingCameraConfig LookingAt(Vector3 target)
        {
            return LookingAt(target, Vector3.Zero);
        }

        public ChasingCameraConfig LookingAt(Vector3 target, Vector3 offset)
        {
            _cameraConfig.LookingAt(target);
            _lookAtOffset = offset;
            return this;
        }

        public ChasingCameraConfig Heading(Vector3 up)
        {
            _cameraConfig.Heading(up);
            return this;
        }

        public ChasingCameraConfig KeepAwayOf(Vector3 offset)
        {
            _positionOffset = offset;
            return this;
        }

        public ChasingCameraConfig Weight(Single mass)
        {
            _mass = mass;
            return this;
        }

        public ChasingCameraConfig WithStiffnessOf(Single stiffness)
        {
            _stiffness = stiffness;
            return this;
        }

        public ChasingCameraConfig WithDampingOf(Single damping)
        {
            _dampingFactor = damping;
            return this;
        }

        public ChasingCameraConfig WithDragOf(Single drag)
        {
            _drag = drag;
            return this;
        }

        public Vector3 PositionOffset
        {
            get { return _positionOffset; }
        }

        public Vector3 LookAtOffset
        {
            get { return _lookAtOffset; }
        }

        public IWorldObject ChasedObject
        {
            get { return _chasedObject; }
        }

        public Single Stiffness
        {
            get { return _stiffness; }
        }
        private Single _stiffness;

        public Single DampingFactor
        {
            get { return _dampingFactor; }
        }
        private Single _dampingFactor;

        public Single Mass
        {
            get { return _mass; }
        }
        private Single _mass;

        public Single DragFactor
        {
            get { return _drag; }
        }
        private Single _drag;

        public Single NearPlaneDistance
        {
            get { return _cameraConfig.NearPlaneDistance; }
        }

        Single ICameraConfig.NearPlaneDistance
        {
            get { return NearPlaneDistance; }
        }

        public Single FarPlaneDistance
        {
            get { return _cameraConfig.FarPlaneDistance; }
        }

        Single ICameraConfig.FarPlaneDistance
        {
            get { return FarPlaneDistance; }
        }

        public Single FieldOfView
        {
            get { return _cameraConfig.FieldOfView; }
        }

        Single ICameraConfig.FieldOfView
        {
            get { return FieldOfView; }
        }

        public Vector3 Position
        {
            get { return _cameraConfig.Position; }
        }

        Vector3 ICameraConfig.Position
        {
            get { return Position; }
        }

        public Vector3 Up
        {
            get { return _cameraConfig.Up; }
        }

        Vector3 ICameraConfig.Up
        {
            get { return Up; }
        }

        public Vector3 Target
        {
            get { return _cameraConfig.Forward; }
        }

        Vector3 ICameraConfig.Forward
        {
            get { return Target; }
        }
    }
}