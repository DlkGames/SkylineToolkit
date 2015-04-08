using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class TabsControl : ControlsContainer
    {
        public TabsControl()
            : this("Tabs")
        {
        }

        public TabsControl(string name)
            : this(name, Vector3.zero, new Vector2(400, 200))
        {
        }

        public TabsControl(string name, Vector3 position, Vector2 size)
        {
            this.Name = name;

            Initialize();

            this.Position = position;
            this.Size = size;
        }

        public string Name { get; set; }

        public float StripHeight
        {
            get
            {
                return this.Strip.Height;
            }
            set
            {
                float oldHeight = StripHeight;

                this.Strip.Height = value;
                this.Container.Position = this.Container.Position + new Vector3(0, value - oldHeight, 0);
            }
        }

        public Vector2 Size
        {
            get {
                return new Vector2(Strip.Width, Strip.Height + Container.Height);
            }
            set {
                Strip.Width = value.x;
                Container.Width = value.x;

                Strip.Height = StripHeight;
                Container.Height = value.y - StripHeight;
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.Strip.Position;
            }
            set
            {
                this.Strip.Position = value;
                this.Container.Position = value + new Vector3(0, StripHeight, 0);
            }
        }

        public TabStrip Strip { get; protected set; }

        public TabContainer Container { get; protected set; }

        protected void Initialize()
        {
            Strip = new TabStrip(Name + "TabStrip");
            Container = new TabContainer(Name + "TabContainer");

            Strip.Container = Container;
        }

        public override void AttachTo(GameObject gameObject)
        {
            Strip.GameObject.transform.parent = gameObject.transform;
            Container.GameObject.transform.parent = gameObject.transform;
        }

        public override void AttachTo(IColossalControl control)
        {
            control.AttachControl(Strip);
            control.AttachControl(Container);
        }

        #region IDisposable

        public override void Dispose()
        {
            if (Strip != null)
            {
                Strip.Dispose();
            }

            if (Container != null)
            {
                Container.Dispose();
            }
        }

        #endregion
    }
}
