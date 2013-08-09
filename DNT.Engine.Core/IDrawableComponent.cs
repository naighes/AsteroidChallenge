using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public interface IDrawableComponent : ISceneComponent, IProcessedByContentPipeline
    {
        void Draw(Matrix view, Matrix projection);
        event EventHandler Drawing;
        event EventHandler Drawn;
        void Hide();
        void Show();
    }
}