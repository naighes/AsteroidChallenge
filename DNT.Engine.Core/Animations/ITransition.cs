using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Animations
{
    public interface ITransition
    {
        Single CurrentTransitionAmount { get; }
        void Update(GameTime gameTime);
        void Reset();
    }
}