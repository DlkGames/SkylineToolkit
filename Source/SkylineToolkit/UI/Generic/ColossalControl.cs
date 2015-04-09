using ColossalFramework.UI;
using System;
using System.Linq;
using UnityEngine;

namespace SkylineToolkit.UI.Generic
{
    public class ColossalControl<T> : ColossalControl, IColossalControl<T>
        where T : UIComponent
    {
        #region Constructors

        public ColossalControl(T component)
            : base(component)
        {
        }

        public ColossalControl(string name)
            : base(name, typeof(T))
        {
        }

        public ColossalControl(IColossalControl control)
        {
            if (control.UIComponent.GetType() != typeof(T))
            {
                throw new InvalidCastException(String.Format("Can't wrap the given ColossalControl<{0}> with ColossalControl<{1}>.", control.GetType().Name, typeof(T).Name));
            }

            this.GameObject = control.GameObject;

            this.UIComponent = (T)control.UIComponent;

            this.SubscribeEvents();
        }

        #endregion

        #region Properties

        #region Components

        public new T UIComponent
        {
            get
            {
                return (T)base.UIComponent;
            }
            protected set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        #region Appearance

        public new IColossalControl TooltipBox
        {
            get
            {
                UIComponent component = this.UIComponent.tooltipBox;

                Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

                return (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });
            }
            set
            {
                this.UIComponent.tooltipBox = value.UIComponent;
            }
        }

        #endregion

        #region State

        public new IColossalControl[] Children
        {
            get
            {
                return this.UIComponent.components.Select(component =>
                {
                    Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

                    return (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });
                }).ToArray();
            }
        }

        public new IColossalControl Parent
        {
            get
            {
                UIComponent component = this.UIComponent.parent;

                Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

                return (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });
            }
        }

        #endregion

        #endregion

        #region Methods

        public new static IColossalControl FromUIComponent(UIComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            Type componentType = GetUIComponentType(component.GetType());
            Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

            return (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });
        }

        public new static TControl FromUIComponent<TControl>(UIComponent component)
            where TControl : IColossalControl
        {
            IColossalControl control = (TControl)ColossalControl<ColossalFramework.UI.UIComponent>.FromUIComponent(component);

            if (!typeof(TControl).IsAssignableFrom(control.GetType()))
            {
                throw new InvalidCastException();
            }

            return (TControl)control;
        }

        #region Wrapping Colossal UI

        /// <summary>
        /// Initializes the Colossal UI wrapper.
        /// </summary>
        /// <param name="name"></param>
        protected override void InitializeComponent(string name, Type componentType)
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

        #endregion

        #region Handling Child Controls

        /// <summary>
        /// Creates a new control of given <paramref name="type"/> and attaches it as a child control.
        /// </summary>
        /// <remarks>
        /// This method accepts both SkylineToolkit and Colossal UI control/component classes.
        /// Original Colossal <see cref="UIComponent"/> types gets wrapped automatically with the control classes from SkylineToolkit.
        /// The returned control requires casting to the desired type.
        /// </remarks>
        /// <param name="type">The type of the new control.</param>
        /// <returns>The newly created control.</returns>
        public new IColossalControl AddControl(Type type)
        {
            if (typeof(ColossalControl<>).IsAssignableFrom(type))
            {
                IColossalControl control = (IColossalControl)Activator.CreateInstance(type);

                this.UIComponent.AddUIComponent(control.UIComponent.GetType());

                return control;
            }

            if (typeof(UIComponent).IsAssignableFrom(type))
            {
                UIComponent component = this.UIComponent.AddUIComponent(type);

                Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

                IColossalControl colossalControl = (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });

                return colossalControl;
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Creates a new control of given type <typeparamref name="TControl"/> and attaches it as a child control.
        /// </summary>
        /// <remarks>
        /// This uses the control classes from original Colossal UI for instantiating the new control but wraps it 
        /// automatically with the control classes from SkylineToolkit.
        /// </remarks>
        /// <typeparam name="TControl">The type of the new control.</typeparam>
        /// <returns>The newly created control.</returns>
        public ColossalControl<TControl> AddComponent<TControl>()
            where TControl : UIComponent
        {
            TControl component = this.UIComponent.AddUIComponent<TControl>();

            return new ColossalControl<TControl>(component);
        }

        /// <summary>
        /// Attaches an already existing UI <paramref name="component"/> as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <remarks>
        /// This wraps the given UI <paramref name="component"/> with the SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="component"></param>
        /// <returns>The attached child control.</returns>
        public new IColossalControl AttachControl(UIComponent component)
        {
            component = this.UIComponent.AttachUIComponent(component.gameObject);

            Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

            IColossalControl colossalControl = (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });

            return colossalControl;
        }

        /// <summary>
        /// Finds a child control matching the given filter.
        /// </summary>
        /// <remarks>
        /// The found <see cref="UIComponent"/>, if there's one, gets automatically wrapped up into a SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="filter">A filter used to search a sub control.</param>
        /// <returns>The found child control or null when found nothin.</returns>
        public new IColossalControl FindChild(string filter)
        {
            UIComponent component = this.UIComponent.Find(filter);

            if (component == null)
            {
                return null;
            }

            Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

            IColossalControl colossalControl = (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });

            return colossalControl;
        }

        /// <summary>
        /// Finds a child control matching the given filter and control type.
        /// </summary>
        /// <remarks>
        /// This uses the SkylineToolkit control wrappers.
        /// The found <see cref="UIComponent"/>, if there's one, gets automatically wrapped up into a SkylineToolkit control wrapper.
        /// </remarks>
        /// <typeparam name="TControl">The type of the expected child control.</typeparam>
        /// <param name="filter">A filter used to search a sub control.</param>
        /// <returns>The found child control or null when found nothin.</returns>
        public TControl FindChild<TControl>(string filter)
            where TControl : IColossalControl
        {
            Type componentType = GetUIComponentType(typeof(TControl));

            UIComponent component = this.UIComponent.Find(filter, componentType);

            if (component == null)
            {
                return default(TControl);
            }

            Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

            IColossalControl colossalControl = (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });

            return (TControl)colossalControl;
        }

        /// <summary>
        /// Finds a child control matching the given filter and control type.
        /// </summary>
        /// <remarks>
        /// The found <see cref="UIComponent"/>, if there's one, gets automatically wrapped up into a SkylineToolkit control wrapper.
        /// </remarks>
        /// <typeparam name="TControl">The type of the expected child control.</typeparam>
        /// <param name="filter">A filter used to search a sub control.</param>
        /// <returns>The found child control or null when found nothin.</returns>
        public IColossalControl FindColossalChild<TControl>(string filter)
           where TControl : UIComponent
        {
            UIComponent component = this.UIComponent.Find<TControl>(filter);

            Type constructedType = typeof(ColossalControl<>).MakeGenericType(component.GetType());

            IColossalControl colossalControl = (IColossalControl)Activator.CreateInstance(constructedType, new object[] { component });

            return colossalControl;
        }

        #endregion

        public void RemoveComponent<TControl>(TControl control)
             where TControl : UIComponent
        {
            this.UIComponent.RemoveUIComponent(control);
        }

        public new IColossalControl GetRootContainer()
        {
            return new ColossalControl<UIComponent>(this.UIComponent.GetRootContainer());
        }

        public new IColossalControl GetRootCustomContainer()
        {
            return new ColossalControl<UIComponent>(this.UIComponent.GetRootCustomControl());
        }

        /// <summary>
        /// Returns the type of the wrapped <see cref="UIComponent"/> from a ColossalControl wrapper.
        /// </summary>
        /// <param name="control">The SkylineToolkit UI Wrapper from which to receive the wrapped type.</param>
        /// <returns>The type of the wrapped <see cref="UIComponent"/>.</returns>
        protected static Type GetUIComponentType(IColossalControl control)
        {
            return GetUIComponentType(control.GetType());
        }

        /// <summary>
        /// Returns the Colossal <see cref="UIComponent"/> <see cref="Type"/> for the given <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// If the given <paramref name="type"/> is a UIComponent, <paramref name="type"/> itself gets returned.
        /// If <paramref name="type"/> is a ColossalControl (SkylineToolkit wrapper) the type of the wrapped
        /// UIComponent will be returned.
        /// For every other type this method will return null.
        /// </remarks>
        /// <param name="type">The initial search type.</param>
        /// <returns>The found UIComponent type.</returns>
        protected static Type GetUIComponentType(Type type)
        {
            if (typeof(UIComponent).IsAssignableFrom(type))
            {
                return type;
            }

            if (!typeof(IColossalControl).IsAssignableFrom(type))
            {
                return null;
            }

            Type colossalControlType = type;

            if (colossalControlType.IsInterface)
            {
                return null;
            }

            while (!colossalControlType.IsGenericType || colossalControlType.GetGenericTypeDefinition() != typeof(ColossalControl<>))
            {
                if (colossalControlType.BaseType == null || colossalControlType == typeof(object))
                {
                    return null;
                }

                colossalControlType = colossalControlType.BaseType;
            }

            Type componentType = colossalControlType.GetGenericArguments()[0];

            return componentType;
        }
        
        #endregion
    }
}
