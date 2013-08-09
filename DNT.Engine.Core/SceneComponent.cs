using System;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public abstract class SceneComponent : ISceneComponent
    {
        protected SceneComponent(Scene scene)
        {
            Verify.That(scene).Named("scene").IsNotNull();
            _scene = scene;
            _id = Guid.NewGuid();
        }

        public Guid Id
        {
            get { return _id; }
        }
        private readonly Guid _id;

        protected Scene Scene
        {
            get { return _scene; }
        }
        private readonly Scene _scene;

        public virtual void Update(GameTime gameTime) { }

        public Boolean MarkedForRemoval
        {
            get { return _markedForRemoval; }
        }
        private Boolean _markedForRemoval;

        public void MarkForRemoval()
        {
            _markedForRemoval = true;
        }
    }
}