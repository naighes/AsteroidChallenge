using System;
using System.Collections;
using System.Collections.Generic;
using DNT.Engine.Core.Exceptions;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core
{
    public class SceneManager
    {
        private readonly IList<Scene> _scenes;
        private Boolean _hasBeenInitialized;
        private Int32 _currentSceneIndex;

        internal SceneManager()
        {
            _scenes = new List<Scene>();
            _postDrawTask = new Queue<Action>();
        }

        public void AddScenes(params Scene[] scenes)
        {
            if (_hasBeenInitialized)
                throw new InvalidOperationException("Cannot add scenes once game has been initialized. Scenes must be added before calling \"base.Initialize()\" method.");

            foreach (var scene in scenes)
                _scenes.Add(scene);
        }

        internal void Initialize()
        {
            _hasBeenInitialized = true;

            if (_scenes.IsNullOrEmpty())
                throw new GameInitializationException("At least one scene is required.");
        }

        internal void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
        }

        internal void Draw()
        {
            CurrentScene.Draw();
            ExecutePostDrawTasks();
        }

        private void ExecutePostDrawTasks()
        {
            try
            {
                while (true)
                {
                    if (_postDrawTask.Count <= 0)
                        return;

                    var task = _postDrawTask.Dequeue();

                    if (task.IsNotNull())
                        task();
                }
            }
            catch (InvalidOperationException) { return; }
        }

        internal void PlayCurrentScene()
        {
            if (_currentSceneIndex.IsLowerThan(0))
                _currentSceneIndex = 0;

            var scene = _scenes[_currentSceneIndex];
            scene.Load();
            scene.OnBeginPlay();
        }

        internal Scene CurrentScene
        {
            get { return _scenes[_currentSceneIndex]; }
        }

        public void PlayNextScene()
        {
            AddPostDrawAction(() =>
                                  {
                                      if ((_currentSceneIndex + 1).IsGreaterThan(_scenes.Count - 1))
                                          throw new IndexOutOfRangeException("There are no more scenes to play at.");

                                      _currentSceneIndex++;
                                      PlayCurrentScene();
                                  });
        }

        protected void AddPostDrawAction(Action action)
        {
            _postDrawTask.Enqueue(action);
        }

        private readonly Queue<Action> _postDrawTask;

        public void PlayPreviousScene()
        {
            AddPostDrawAction(() =>
            {
                if ((_currentSceneIndex - 1).IsLowerThan(0))
                    throw new IndexOutOfRangeException("There are no more scenes to play at.");

                _currentSceneIndex--;
                PlayCurrentScene();
            });
        }
    }
}