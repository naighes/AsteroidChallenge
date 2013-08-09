using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras
{
    public class CameraConfig : ICameraConfig
    {
        private Single _nearPlaneDistance;
        private Single _farPlaneDistance;
        private Single _fieldOfView;

        private Vector3 _position;
        private Vector3 _forward;
        private Vector3 _up;

        internal CameraConfig()
        {
            _nearPlaneDistance = 0.1f;
            _farPlaneDistance = 1000.0f;
            _fieldOfView = MathHelper.PiOver4;

            _position = Vector3.Zero;
            _forward = Vector3.Forward;
            _up = Vector3.Up;
        }

        public CameraConfig Between(Single nearPlaneDistance, Single farPlaneDistance)
        {
            _nearPlaneDistance = nearPlaneDistance;
            _farPlaneDistance = farPlaneDistance;
            return this;
        }

        public Vector3 Up
        {
            get { return _up; }
        }

        public Vector3 Forward
        {
            get { return _forward; }
        }

        public Vector3 Position
        {
            get { return _position; }
        }

        public Single FieldOfView
        {
            get { return _fieldOfView; }
        }

        public Single FarPlaneDistance
        {
            get { return _farPlaneDistance; }
        }

        public Single NearPlaneDistance
        {
            get { return _nearPlaneDistance; }
        }

        public CameraConfig Wide(Single fieldOfView)
        {
            _fieldOfView = fieldOfView;
            return this;
        }

        public CameraConfig At(Vector3 position)
        {
            _position = position;
            return this;
        }

        public CameraConfig LookingAt(Vector3 target)
        {
            _forward = target;
            return this;
        }

        public CameraConfig Heading(Vector3 up)
        {
            _up = up;
            return this;
        }
    }
}