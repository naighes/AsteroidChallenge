using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Cameras
{
    public abstract class CameraBase : SceneComponent, ICamera
    {
        protected CameraBase(Scene scene, ICameraConfig config)
            : base(scene)
        {
            _fieldOfView = config.FieldOfView;

            _nearPlaneDistance = config.NearPlaneDistance;
            _farPlaneDistance = config.FarPlaneDistance;

            _originalPosition = _position = config.Position;
            _originalForward = config.Forward;
            SetTarget(_originalForward);
            _originalUp = _up = config.Up;
        }

        private readonly Single _fieldOfView;
        private readonly Single _nearPlaneDistance;
        private readonly Single _farPlaneDistance;

        public Matrix View
        {
            get
            {
                if (_needsUpdateView)
                {
                    Matrix.CreateLookAt(ref _position, ref _target, ref _up, out _view);
                    _needsUpdateView = false;
                    _needsUpdateFrustum = true;
                }

                return _view;
            }
        }
        private Matrix _view;
        private Boolean _needsUpdateView = true;

        public Matrix ViewProjection
        {
            get { return View*Projection; }
        }

        public Matrix InvertedViewProjection
        {
            get { return Matrix.Invert(ViewProjection); }
        }

        public Matrix Projection
        {
            get
            {
                if (_needsUpdateProjection)
                {
                    Matrix.CreatePerspectiveFieldOfView(_fieldOfView,
                                                        Scene.Viewport.AspectRatio,
                                                        _nearPlaneDistance,
                                                        _farPlaneDistance,
                                                        out _projection);
                    _needsUpdateProjection = false;
                    _needsUpdateFrustum = true;
                }

                return _projection;
            }
        }
        private Matrix _projection;
        private Boolean _needsUpdateProjection = true;

        public Vector3 Position
        {
            get { return _position; }
        }
        private Vector3 _position;

        protected Vector3 OriginalPosition
        {
            get { return _originalPosition; }
        }
        private readonly Vector3 _originalPosition;

        public BoundingFrustum BoundingFrustum
        {
            get
            {
                if (_needsUpdateFrustum)
                {
                    _boundingFrustum = new BoundingFrustum(View * Projection);
                    _needsUpdateFrustum = false;
                }

                return _boundingFrustum;
            }
        }
        private BoundingFrustum _boundingFrustum;
        private Boolean _needsUpdateFrustum = true;

        public Vector3 Target
        {
            get { return _target; }
        }
        private Vector3 _target;

        protected Vector3 OriginalForward
        {
            get { return _originalForward; }
        }
        private readonly Vector3 _originalForward;

        public Vector3 Up
        {
            get { return _up; }
        }
        private Vector3 _up;

        protected Vector3 OriginalUp
        {
            get { return _originalUp; }
        }
        private readonly Vector3 _originalUp;

        public Vector3 Right
        {
            get
            {
                _right = Vector3.Normalize(Vector3.Cross(Forward, _up));
                return _right;
            }
        }
        private Vector3 _right;

        protected void SetPosition(Vector3 position)
        {
            if (_position == position)
                return;

            _position = position;
            _needsUpdateView = true;
        }

        protected void SetTarget(Vector3 target)
        {
            if (_target == target)
                return;

            _target = target;
            _needsUpdateView = true;
        }

        protected void SetUp(Vector3 up)
        {
            if (_up == up)
                return;

            _up = up;
            _up.Normalize();
            _needsUpdateView = true;
        }

        public Vector3 Forward
        {
            get { return Vector3.Normalize(_target - _position); }
        }
    }
}