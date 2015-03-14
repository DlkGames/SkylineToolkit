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
        private T colossalUIComponent;

        public ColossalControl(T component)
        {
            this.GameObject = component.gameObject;

            this.UIComponent = component;
        }

        public ColossalControl(string name)
        {
            InitializeComponent(name);
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

        public virtual void InitializeComponent(string name)
        {
            this.GameObject = new GameObject(name, typeof(T));

            this.IsActive = false;

            this.UIComponent = this.GameObject.GetComponent<T>();
        }

        public TControl AddControl<TControl>()
            where TControl : ColossalControl<T>, new()
        {
            TControl control = new TControl();

            this.UIComponent.AddUIComponent(control.UIComponent.GetType());

            return control;
        }

        // Fit Children
    }
}
