using System;
using System.Collections.Generic;


#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public class GameObject : Object
    {
        #region Private fields
        private readonly List<Component> _components = new List<Component>();
        #endregion

        #region Properties
        public Transform transform
        {
            get
            {
                return this.GetComponent<Transform>();
            }
        }

        public Rigidbody rigidbody
        {
            get
            {
                return this.GetComponent<Rigidbody>();
            }
        }

        public Light light
        {
            get
            {
                return this.GetComponent<Light>();
            }
        }
        #endregion

        #region Constructor
        public GameObject()
        {
            name = "";
            GameObject.SetupGameObject(this);
        }

        public GameObject(string name)
        {
            this.name = name;
            GameObject.SetupGameObject(this);
        }

        public GameObject(string name, params Type[] components)
        {
            this.name = name;
            GameObject.SetupGameObject(this);

            foreach (var component in components)
            {
                this.AddComponent(component);
            }
        }
        #endregion

        #region Internal API
        internal static void SetupGameObject(GameObject gameObj, bool isRuntime = true)
        {
            if (isRuntime)
            {
                gameObj.IsPrefab = false;
                gameObj.AddComponent(typeof(Transform));
            }
            else
            {
                gameObj.IsPrefab = true;
            }

            DlkGames.Unity.TestHelpers.TestableUnity.Internal.EngineControl.Instance.Add(gameObj);
        }
        #endregion

        #region API
        public Component AddComponent<T>()
            where T : Component
        {
            return this.AddComponent(typeof(T)) as T;
        }

        public Component AddComponent(Type type)
        {
            this.AssertNull();

            var component = Activator.CreateInstance(type, true) as Component;

            if (component == null)
            {
                throw new ArgumentException("type needs to be a Component");
            }

            component.IsPrefab = false;
            this._components.Add(component);

            try
            {
                component.DoAwake();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }

            return component;
        }

        public T GetComponent<T>()
            where T : Component
        {
            return this.GetComponent(typeof(T)) as T;
        }

        public Component GetComponent(string type)
        {
            return this.GetComponent(Type.GetType(type));
        }

        public Component GetComponent(Type type)
        {
            this.AssertNull();

            Component foundComponent = null;

            while (!foundComponent)
            {
                var ind = this._components.FindIndex(c => c.GetType() == type);

                if (ind == -1)
                {
                    return null;
                }

                foundComponent = this._components[ind];

                if (foundComponent)
                {
                    return foundComponent;
                }

                // The component is supposed to be destroyed, so remove it
                this._components.RemoveAt(ind);
                foundComponent = null;
            }

            return foundComponent;
        }

        internal void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public T GetComponentInChildren<T>() where T : Component
        {
            return (T)this.GetComponentInChildren(typeof(T));
        }
        public Component GetComponentInChildren(Type type)
        {
            this.AssertNull();

            //TODO: recursively walk heirarchy

            throw new NotImplementedException();
        }

        public T[] GetComponentsInChildren<T>(bool includeInactive = false) where T : Component
        {
            return this.GetComponentsInChildren(typeof(T), includeInactive) as T[];
        }

        public Component[] GetComponentsInChildren(Type t, bool includeInactive = false)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        public void SetActive(bool value)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        public bool CompareTag(string tag)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        public void SendMessageUpwards(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            //TODO: walk up heiarchy, doing sendmessage on each gameobject

            throw new NotImplementedException();
        }

        /// <summary>
        /// Run the specified method name on all components on this gameobject
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="value"></param>
        /// <param name="options">if requirereceiver, and no methods were run, throw an error</param>
        public void SendMessage(string methodName, object value = null,
            SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        public void BroadcastMessage(string methodName, object parameter = null,
            SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        public void SampleAnimation(AnimationClip animation, float time)
        {
            this.AssertNull();

            throw new NotImplementedException();
        }

        /// <summary>
        /// run Update on all components
        /// </summary>
        internal void RunComponentUpdates()
        {
            foreach (var component in this._components)
            {
                component.DoUpdate();
            }
        }

        /// <summary>
        /// run LateUpdate on all components
        /// </summary>
        internal void RunComponentLateUpdates()
        {
            foreach (var component in this._components)
            {
                component.DoLateUpdate();
            }
        }
        #endregion

        #region Static API
        public static GameObject FindWithTag(string tag)
        {
            return FindGameObjectWithTag(tag);
        }

        public static GameObject FindGameObjectWithTag(string tag)
        {
            throw new NotImplementedException();
        }

        public static GameObject[] FindGameObjectsWithTag(string tag)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}