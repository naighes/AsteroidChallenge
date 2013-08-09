using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Graphics
{
    public class SpriteEventArgs : EventArgs
    {
        public Vector2 ClickCoordinates
        {
            get { return _clickCoordinates; }
        }
        private readonly Vector2 _clickCoordinates;

        internal SpriteEventArgs(Vector2 clickCoordinates)
        {
            _clickCoordinates = clickCoordinates;
        }
    }
}