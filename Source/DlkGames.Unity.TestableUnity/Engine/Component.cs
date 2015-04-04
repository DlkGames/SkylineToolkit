using System;

#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    /// <summary>
    /// 
    /// </summary>
    public class Component : Object
    {
        public GameObject gameObject
        {
            get;
            internal set;
        }

        internal Component()
        {
        }

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

        public Camera camera
        {
            get
            {
                return this.GetComponent<Camera>();
            }
        }

        public Light light
        {
            get
            {
                return this.GetComponent<Light>();
            }
        }

        public Animation animation
        {
            get
            {
                return this.GetComponent<Animation>();
            }
        }

        public ConstantForce constantForce
        {
            get
            {
                return this.GetComponent<ConstantForce>();
            }
        }

        protected virtual void CUpdate()
        {
        }

        protected virtual void CLateUpdate()
        {
        }

        protected virtual void CAwake()
        {
        }

        protected virtual void CStart()
        {
        }

        internal void DoAwake()
        {
            try
            {
                CAwake();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        internal void DoStart()
        {
            try
            {
                CStart();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        internal void DoUpdate()
        {
            try
            {
                CUpdate();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        internal void DoLateUpdate()
        {
            try
            {
                CLateUpdate();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        public T GetComponent<T>()
            where T : Component
        {
            return this.GetComponent(typeof(T)) as T;
        }

        public Component GetComponent(Type type)
        {
            this.AssertNull();

            return this.gameObject.GetComponent(type);
        }

        public Component GetComponent(string type)
        {
            this.AssertNull();
            return this.gameObject.GetComponent(type);
        }

        public T GetComponentInChildren<T>() where T : Component
        {
            return this.GetComponentInChildren(typeof(T)) as T;
        }

        private Component GetComponentInChildren(Type type)
        {
            this.AssertNull();

            return this.gameObject.GetComponentInChildren(type);
        }

        public void SendMessageUpwards(string methodName, object value = null,
            SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            this.gameObject.SendMessageUpwards(methodName, value, options);
        }

        public void SendMessage(string methodName, object value = null,
            SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            this.gameObject.SendMessage(methodName, value, options);
        }

        public void BroadcastMessage(string methodName, object value = null,
            SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            this.AssertNull();

            this.gameObject.BroadcastMessage(methodName, value, options);
        }
    }
}
