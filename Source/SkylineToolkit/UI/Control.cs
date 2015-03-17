using UnityEngine;

namespace SkylineToolkit.UI
{
    public abstract class Control : IControl
    {
        private GameObject gameObject;

        public Control()
        {
        }

        public Control(IControl control)
        {
            this.GameObject = control.GameObject;
        }

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
            protected set
            {
                this.gameObject = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.GameObject.activeSelf;
            }
            set
            {
                this.GameObject.SetActive(value);
            }
        }
    }
}
