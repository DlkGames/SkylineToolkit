using System;
using System.Collections.Generic;

namespace DlkGames.Unity.TestHelpers.TestableUnity.Internal
{
#if !NOUNITY
    using UnityEngine;
    using Object = UnityEngine.Object;
#endif

    public sealed class EngineControl
    {
        #region Private fields
        private readonly System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();
        private readonly IntDictionary<GameObject> _gameObjects = new IntDictionary<GameObject>();
        private readonly IList<int> _keysToRemove = new List<int>();

        private static EngineControl _instance;
        #endregion

        #region Propeties
        public static EngineControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EngineControl();

                    _instance.Setup();
                }

                return _instance;
            }
        }
        #endregion

        #region Constructor
        public EngineControl()
        {
            if (EngineControl._instance != null)
            {
                throw new InvalidOperationException("EngineControl already initialized.");
            }
        }
        #endregion

        #region Static API
        public static EngineControl RenewEngineControl()
        {
            EngineControl._instance = null;

            return EngineControl.Instance;
        }
        #endregion

        #region Internal API
        private void Setup()
        {
            Time.Update(0);
        }

        internal void Add(GameObject gobj)
        {
            var id = _gameObjects.Add(gobj);
            gobj.ReferenceData = new ReferenceData
            {
                InstanceID = id
            };
        }
        #endregion

        #region API
        /// <summary>
        /// Go to the next frame and trigger the update method of every Component of all GameObjects.
        /// </summary>
        public void Update()
        {
            this.Update(null);
        }

        /// <summary>
        /// Go to the next frame and trigger the update method of every Component of all GameObjects.
        /// </summary>
        /// <param name="newTime">The elapsed time since the start of the application. Uses the start of the current process if null.</param>
        public void Update(float? newTime)
        {
            if (newTime == null)
            {
                Time.Update((float)_watch.Elapsed.TotalSeconds);
            }
            else
            {
                Time.Update(newTime.Value);
            }


            //TODO: call update on everything
            _keysToRemove.Clear();

            for (int i = 0; i < _gameObjects.Capacity; i++)
            {
                GameObject gobj;
                if (_gameObjects.TryGetValue(i, out gobj))
                {
                    if (Object.IsNull(gobj))
                    {
                        _gameObjects.Remove(i);
                    }
                    else
                    {
                        gobj.RunComponentUpdates();
                    }
                }
            }

            foreach (var key in _keysToRemove)
            {
                _gameObjects.Remove(key);
            }

            _keysToRemove.Clear();

            //TODO: call lateupdate on everything
        }
        #endregion
    }
}