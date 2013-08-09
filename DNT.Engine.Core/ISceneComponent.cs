using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public interface ISceneComponent
    {
        void Update(GameTime gameTime);
        Boolean MarkedForRemoval { get; }
    }
}