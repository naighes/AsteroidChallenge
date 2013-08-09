using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework.Input.Touch;

namespace DNT.Engine.Core.Messaging
{
    public struct TapGesture
    {
        public TapGesture(GestureSample gestureSample)
        {
            Verify.That(gestureSample).Named("gestureSample").IsNotNull();
            _gestureSample = gestureSample;
        }

        public GestureSample GestureSample
        {
            get { return _gestureSample; }
        }
        private GestureSample _gestureSample;
    }
}