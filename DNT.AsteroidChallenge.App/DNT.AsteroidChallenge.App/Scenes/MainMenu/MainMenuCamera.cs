using System;
using DNT.Engine.Core;
using DNT.Engine.Core.Cameras;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public class MainMenuCamera : CameraBase
    {
        public MainMenuCamera(Scene scene, ICameraConfig config)
            : base(scene, config)
        {
        }

        private Single _yawAmount;
        private Single _rollAmount;
        private Matrix _rotation;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _yawAmount += 0.001f;
            _rollAmount += 0.002f;
            _rotation = Matrix.CreateRotationY(_yawAmount) * Matrix.CreateRotationZ(_rollAmount);

            SetTarget(Vector3.Transform(OriginalForward, _rotation));
            SetUp(Vector3.Transform(OriginalUp, _rotation));
        }
    }
}