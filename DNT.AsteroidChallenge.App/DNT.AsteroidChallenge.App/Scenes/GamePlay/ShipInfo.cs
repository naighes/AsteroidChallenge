using System;
using Microsoft.Xna.Framework;

namespace DNT.AsteroidChallenge.App
{
    public struct ShipInfo
    {
        public Single Mass;
        public Single EngineMaximumTorque;
        public Single RotationRate;
        public Single DragFactor;
        public Vector3 OriginalUp;
        public Vector3 OriginalForward;
    }
}