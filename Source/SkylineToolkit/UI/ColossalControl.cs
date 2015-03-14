using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class ColossalControl<T> : Control, IColossalControl
        where T : UIComponent
    {
        private static UIView uiView;

        #region Events

        public delegate void MouseEventHandler(object sender, MouseEventArgs e);

        public event MouseEventHandler Click;

        #endregion

        private T colossalUIComponent;

        public ColossalControl(T component)
        {
            this.GameObject = component.gameObject;

            this.UIComponent = component;

            this.SubscribeEvents();
        }

        public ColossalControl(string name)
        {
            this.InitializeComponent(name);

            this.SubscribeEvents();
        }

        public static UIView ColossalUIView
        {
            get
            {
                if (uiView == null)
                {
                    UIView foundUIView = GameObject.FindObjectOfType<UIView>();

                    if (foundUIView != null)
                    {
                        uiView = foundUIView;
                    }
                }

                return uiView;
            }
        }

        public T UIComponent
        {
            get
            {
                return colossalUIComponent;
            }
            protected set
            {
                colossalUIComponent = value;
            }
        }

        UIComponent IColossalControl.UIComponent
        {
            get
            {
                return this.UIComponent;
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.UIComponent.transformPosition;
            }
            set
            {
                this.UIComponent.transformPosition = value;
            }
        }

        public Vector3 AbsolutePosition
        {
            get
            {
                return this.UIComponent.absolutePosition;
            }
            set
            {
                this.UIComponent.absolutePosition = value;
            }
        }

        public Vector3 RelativePosition
        {
            get
            {
                return this.UIComponent.relativePosition;
            }
            set
            {
                this.UIComponent.relativePosition = value;
            }
        }

        public virtual void InitializeComponent(string name)
        {
            if (ColossalUIView == null)
            {
                Log.Warning("No Colossal UIView found in game.");

                throw new InvalidOperationException("No UIView found in game.");
            }

            this.GameObject = new GameObject(name, typeof(T));

            this.GameObject.transform.parent = ColossalUIView.transform;

            this.IsActive = false;

            this.UIComponent = this.GameObject.GetComponent<T>();
        }

        private void SubscribeEvents()
        {
            this.UIComponent.eventClick += OnClick;
        }

        public TControl AddControl<TControl>()
            where TControl : ColossalControl<T>, new()
        {
            TControl control = new TControl();

            this.UIComponent.AddUIComponent(control.UIComponent.GetType());

            return control;
        }

        // Fit Children

        #region On Events

        private void OnClick(UIComponent component, UIMouseEventParameter e)
        {
            if (this.Click != null)
            {
                MouseEventArgs args = new MouseEventArgs((MouseButtons)e.buttons, e.clicks, e.ray, e.position, e.moveDelta, e.wheelDelta);

                this.Click(this, args);
            }
        }

        #endregion
    }
}
