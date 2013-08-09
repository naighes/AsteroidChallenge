using System;
using System.Collections.Generic;
using System.Linq;
using DNT.Engine.Core.Audio;
using DNT.Engine.Core.Cameras;
using DNT.Engine.Core.CollisionsDetection;
using DNT.Engine.Core.Graphics;
using DNT.Engine.Core.Input;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core
{
    public abstract class Scene
    {
        protected Scene(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider)
        {
            Verify.That(graphicsDevice).Named("graphicsDevice").IsNotNull();
            _graphicsDevice = graphicsDevice;

            Verify.That(serviceProvider).Named("serviceProvider").IsNotNull();
            _serviceProvider = serviceProvider;

            _components = new List<ISceneComponent>();
            _groups = new Dictionary<DrawableComponentGroup, IList<IDrawableComponent>>
                          {
                              {new DrawableComponentGroup("___DefaultGroup___", 0), new List<IDrawableComponent>()}
                          };
            _collisionsMap = new Dictionary<Type, Type[]>();
        }

        public void AddCollisionMap(Type source, Type[] targets)
        {
            _collisionsMap.Add(source, targets);
        }
        private readonly IDictionary<Type, Type[]> _collisionsMap;

        public IInputManager InputManager
        {
            get { return _serviceProvider.GetService(typeof(IInputManager)) as IInputManager; }
        }

        protected internal IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }
        private readonly IServiceProvider _serviceProvider;

        public SoundManager SoundManager
        {
            get { return _serviceProvider.GetService(typeof(SoundManager)) as SoundManager; }
        }

        public Gamer CurrentGamer
        {
            get { return Gamer.SignedInGamers.Count == 0 ? null : Gamer.SignedInGamers[0]; }
        }

        public IEffectConfigurer EffectConfigurer
        {
            get { return _serviceProvider.GetService(typeof(IEffectConfigurer)) as IEffectConfigurer; }
        }

        public void AddGroup(String name)
        {
            Verify.That(name).Named("name").IsNotNullOrWhiteSpace();
            var group = new DrawableComponentGroup(name, _groups.Count + 1);

            if (_groups.ContainsKey(group))
                throw new InvalidOperationException(String.Format("Group named '{0}' has been already added.", group.Name));

            _groups.Add(group, new List<IDrawableComponent>());
        }

        private void AddComponentInternal(ISceneComponent component)
        {
            Verify.That(component).Named("component").IsNotNull();

            if (_components.Contains(component))
                throw new InvalidOperationException("Component is already part of this scene.");

            _components.Add(component);
        }

        private void RemoveComponentInternal(ISceneComponent component)
        {
            Verify.That(component).Named("component").IsNotNull();

            if (!_components.Contains(component))
                throw new InvalidOperationException("Component is missing.");

            _components.Remove(component);
        }

        public void AddComponent(ISceneComponent component)
        {
            if (component is IDrawableComponent)
                AddComponent((IDrawableComponent)component);
            else
                AddComponentInternal(component);
        }

        private void AddComponent(IDrawableComponent component)
        {
            AddComponent(component, _groups.First().Key);
        }

        private void AddComponent(IDrawableComponent component, DrawableComponentGroup group)
        {
            Verify.That(group).Named("group").IsNotNull();

            AddComponentInternal(component);

            if (!_groups.ContainsKey(group))
                throw new InvalidOperationException(String.Format("Cannot find a group named '{0}' into this scene.", group.Name));

            if (_groups[group].Contains(component))
                throw new InvalidOperationException(String.Format("Current drawable component has been already added to group named '{0}'.", group.Name));

            _groups[group].Add(component);
        }

        public void AddComponent(IDrawableComponent component, String groupName)
        {
            Verify.That(groupName).Named("groupName").IsNotNullOrWhiteSpace();
            AddComponent(component, _groups.Single(g => g.Key.Name == groupName).Key);
        }

        public void RemoveComponent(ISceneComponent component)
        {
            if (component is IDrawableComponent)
                RemoveComponent((IDrawableComponent)component);
            else
                RemoveComponentInternal(component);
        }

        private void RemoveComponent(IDrawableComponent component)
        {
            RemoveComponentInternal(component);

            foreach (var group in _groups)
                if (group.Value.Contains(component))
                    group.Value.Remove(component);
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }
        private readonly GraphicsDevice _graphicsDevice;

        protected internal virtual void Update(GameTime gameTime)
        {
            for (var i = 0; i < _components.Count; i++)
                if (_components[i].MarkedForRemoval)
                    RemoveComponent(_components[i]);

            CheckCollisions();

            InputManager.GesturesLookup();

            for (var i = 0; i < _components.Count; i++)
                _components[i].Update(gameTime);
        }

        protected void RemoveComponents<T>()
        {
            foreach (var group in _groups)
                for (var i = group.Value.Count - 1; i >= 0; i--)
                    if (group.Value[i].GetType() == typeof(T))
                        group.Value.RemoveAt(i);
        }

        private void CheckCollisions()
        {
            var collidables = _components.OfType<ICollidable>();

            foreach (var map in _collisionsMap)
            {
                var map1 = map;
                var sources = collidables.Where(c => c.GetType() == map1.Key);
                var map2 = map;
                var targets = collidables.Where(c => map2.Value.Contains(c.GetType()));

                foreach (var source in sources)
                    foreach (var target in targets)
                        source.CheckCollision(target);
            }
        }

        protected internal virtual void Load()
        {
            foreach (var component in _components.OfType<IProcessedByContentPipeline>())
                component.Load();
        }

        protected internal virtual void OnBeginPlay() { }

        private readonly IList<ISceneComponent> _components;

        protected internal virtual void Draw()
        {
            foreach (var group in _groups)
                foreach (var component in group.Value)
                    component.Draw(CurrentCamera.View, CurrentCamera.Projection);
        }

        public SpriteBatch SpriteBatch
        {
            get { return _serviceProvider.GetService(typeof(SpriteBatch)) as SpriteBatch; }
        }

        public SceneManager SceneManager
        {
            get { return _serviceProvider.GetService(typeof(SceneManager)) as SceneManager; }
        }

        public ContentManager Content
        {
            get { return _serviceProvider.GetService(typeof(ContentManager)) as ContentManager; }
        }

        public Viewport Viewport
        {
            get { return GraphicsDevice.Viewport; }
        }

        protected void SetCurrentCamera(ICamera camera)
        {
            _currentCamera = camera;
        }

        public ICamera CurrentCamera
        {
            get { return _currentCamera; }
        }
        private ICamera _currentCamera;

        private readonly IDictionary<DrawableComponentGroup, IList<IDrawableComponent>> _groups;
    }
}