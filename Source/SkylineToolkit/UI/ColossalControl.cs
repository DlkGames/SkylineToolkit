using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class ColossalControl : Control, IColossalControl, IComparable<IColossalControl>
    {
        private static UIView uiView;

        private UIComponent colossalUIComponent;

        #region Events

        #region Mouse actions

        public event MouseEventHandler Click;

        public event MouseEventHandler DisabledClick;

        public event MouseEventHandler DoubleClick;

        public event MouseEventHandler MouseDown;

        public event MouseEventHandler MouseUp;

        public event MouseEventHandler MouseWheel;

        public event MouseEventHandler MouseEnter;

        public event MouseEventHandler MouseLeave;

        public event MouseEventHandler MouseHover;

        public event MouseEventHandler MouseMove;

        public event MouseEventHandler DragStart;

        public event MouseEventHandler DragEnd;

        public event MouseEventHandler DragEnter;

        public event MouseEventHandler DragLeave;

        public event MouseEventHandler DragOver;

        public event MouseEventHandler DragDrop;

        public event MouseEventHandler TooltipEnter;

        public event MouseEventHandler TooltipLeave;

        #endregion

        #region Keyboard actions

        public event KeyboardEventHandler KeyDown;

        public event KeyboardEventHandler KeyUp;

        public event KeyboardEventHandler KeyPress;

        #endregion

        #region Child controls

        public event ChildControlEventHandler ChildControlAdded;

        public event ChildControlEventHandler ChildControlRemoved;

        #endregion

        #region Positioning and Scaling

        public event PropChangedEventHandler<PositionAnchor> AnchorChanged;

        public event FitChildrenEventHandler FitChildren;

        public event PropChangedEventHandler<Vector2> PositionChanged;

        public event PropChangedEventHandler<Vector2> SizeChanged;

        #endregion

        #region Appearance

        public event PropChangedEventHandler<Color32> ColorChanged;

        public event PropChangedEventHandler<Color32> DisabledColorChanged;

        public event PropChangedEventHandler<float> OpacityChanged;

        public event PropChangedEventHandler<PivotPoint> PivotChanged;

        public event TooltipEventHandler TooltipShow;

        public event TooltipEventHandler TooltipHide;

        public event PropChangedEventHandler<string> TooltipTextChanged;

        public event PropChangedEventHandler<int> ZOrderChanged;

        #endregion

        #region State

        public event FocusEventHandler Focus;

        public event FocusEventHandler Focused;

        public event FocusEventHandler Unfocus;

        public event FocusEventHandler Unfocused;

        public event PropChangedEventHandler<bool> Enabled;

        public event PropChangedEventHandler<bool> Disabled;

        public event PropChangedEventHandler<bool> VisibilityChanged;

        public event PropChangedEventHandler<int> TabIndexChanged;

        #endregion

        #endregion

        #region Constructors

        internal ColossalControl()
        {
        }

        public ColossalControl(UIComponent component)
        {
            this.UIComponent = component;

            this.SubscribeEvents();
        }

        public ColossalControl(string name, Type componentType)
        {
            this.InitializeComponent(name, componentType);

            this.SubscribeEvents();
        }

        public ColossalControl(IColossalControl control)
        {
            this.UIComponent = control.UIComponent;

            this.SubscribeEvents();
        }

        #endregion

        #region Properties

        // TODO: Wrapper for UIView ?
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

        #region Component

        public UIComponent UIComponent
        {
            get
            {
                return colossalUIComponent;
            }
            protected set
            {
                colossalUIComponent = value;

                this.GameObject = value.gameObject;
            }
        }

        public string Name
        {
            get
            {
                return this.UIComponent.gameObject.name;
            }
            set
            {
                this.UIComponent.gameObject.name = value;
            }
        }

        #endregion

        #region Positioning and Scaling

        public PositionAnchor Anchor
        {
            get
            {
                return (PositionAnchor)this.UIComponent.anchor;
            }
            set
            {
                this.UIComponent.anchor = (UIAnchorStyle)value;
            }
        }

        public Vector3 ArbitaryPivotOffset
        {
            get
            {
                return this.UIComponent.arbitraryPivotOffset;
            }
            set
            {
                this.UIComponent.arbitraryPivotOffset = value;
            }
        }

        public Vector4 Area
        {
            get
            {
                return this.UIComponent.area;
            }
            set
            {
                this.UIComponent.area = value;
            }
        }

        public Vector4 Limits
        {
            get
            {
                return this.UIComponent.limits;
            }
            set
            {
                this.UIComponent.limits = value;
            }
        }

        public Vector2 MaximumSize
        {
            get
            {
                return this.UIComponent.maximumSize;
            }
            set
            {
                this.UIComponent.maximumSize = value;
            }
        }

        public Vector2 MinimumSize
        {
            get
            {
                return this.UIComponent.minimumSize;
            }
            set
            {
                this.UIComponent.minimumSize = value;
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

        public bool IsClippedFromParent
        {
            get
            {
                return this.UIComponent.IsClippedFromParent();
            }
        }

        public virtual bool IsAutoSize
        {
            get
            {
                return this.UIComponent.autoSize;
            }
            set
            {
                this.UIComponent.autoSize = value;
            }
        }

        public Vector3 Center
        {
            get
            {
                return this.UIComponent.center;
            }
        }

        public bool IsClippingChildren
        {
            get
            {
                return this.UIComponent.clipChildren;
            }
            set
            {
                this.UIComponent.clipChildren = value;
            }
        }

        public float Height
        {
            get
            {
                return this.UIComponent.height;
            }
            set
            {
                this.UIComponent.height = value;
            }
        }

        public float Width
        {
            get
            {
                return this.UIComponent.width;
            }
            set
            {
                this.UIComponent.width = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return this.UIComponent.size;
            }
            set
            {
                this.UIComponent.size = value;
            }
        }

        public int ZOrder
        {
            get
            {
                return this.UIComponent.zOrder;
            }
            set
            {
                this.UIComponent.zOrder = value;
            }
        }

        #endregion

        #region Appearance

        public AudioClip ClickSound
        {
            get
            {
                return this.UIComponent.clickSound;
            }
            set
            {
                this.UIComponent.clickSound = value;
            }
        }

        public AudioClip DisabledClickSound
        {
            get
            {
                return this.UIComponent.disabledClickSound;
            }
            set
            {
                this.UIComponent.disabledClickSound = value;
            }
        }

        public Color32 Color
        {
            get
            {
                return this.UIComponent.color;
            }
            set
            {
                this.UIComponent.color = value;
            }
        }

        public Color32 DisabledColor
        {
            get
            {
                return this.UIComponent.disabledColor;
            }
            set
            {
                this.UIComponent.disabledColor = value;
            }
        }

        public CursorInfo HoverCursorStyle
        {
            get
            {
                return CursorInfo.FromColossalCursorInfo(this.UIComponent.hoverCursor);
            }
            set
            {
                this.UIComponent.hoverCursor = value.ToColossalCursorInfo();
            }
        }

        public float Opacity
        {
            get
            {
                return this.UIComponent.opacity;
            }
            set
            {
                this.UIComponent.opacity = value;
            }
        }

        public PivotPoint Pivot
        {
            get
            {
                return (PivotPoint)this.UIComponent.pivot;
            }
            set
            {
                this.UIComponent.pivot = (UIPivotPoint)value;
            }
        }

        public string TooltipText
        {
            get
            {
                return this.UIComponent.tooltip;
            }
            set
            {
                this.UIComponent.tooltip = value;
            }
        }

        public TooltipAnchor TooltipAnchor
        {
            get
            {
                return (TooltipAnchor)this.UIComponent.tooltipAnchor;
            }
            set
            {
                this.UIComponent.tooltipAnchor = (UITooltipAnchor)value;
            }
        }

        public IColossalControl TooltipBox
        {
            get
            {
                UIComponent component = this.UIComponent.tooltipBox;

                return new ColossalControl(component);
            }
            set
            {
                this.UIComponent.tooltipBox = value.UIComponent;
            }
        }

        #endregion

        #region State

        public bool IsEnabled
        {
            get
            {
                return this.UIComponent.isEnabled;
            }
            set
            {
                if (value)
                {
                    this.UIComponent.Enable();
                }
                else
                {
                    this.UIComponent.Disable();
                }
            }
        }

        public bool HasFocus
        {
            get
            {
                return this.UIComponent.hasFocus;
            }
            set
            {
                if (value)
                {
                    this.UIComponent.Focus();
                }
                else
                {
                    this.UIComponent.Unfocus();
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.UIComponent.isVisible;
            }
            set
            {
                if (value)
                {
                    this.UIComponent.Show();
                }
                else
                {
                    this.UIComponent.Hide();
                }
            }
        }

        public bool IsVisibleSelf
        {
            get
            {
                return this.UIComponent.isVisibleSelf;
            }
        }

        public bool IsVisibleInParent
        {
            get
            {
                return this.UIComponent.IsVisibleInParent();
            }
        }

        public bool IsInteractive
        {
            get
            {
                return this.UIComponent.isInteractive;
            }
            set
            {
                this.UIComponent.isInteractive = value;
            }
        }

        public bool IsTooltipOnTop
        {
            get
            {
                return this.UIComponent.bringTooltipToFront;
            }
            set
            {
                this.UIComponent.bringTooltipToFront = value;
            }
        }

        public bool IsLocalized
        {
            get
            {
                return this.UIComponent.isLocalized;
            }
            set
            {
                this.UIComponent.isLocalized = value;
            }
        }

        public bool IsTooltipLocalized
        {
            get
            {
                return this.UIComponent.isTooltipLocalized;
            }
            set
            {
                this.UIComponent.isTooltipLocalized = value;
            }
        }

        public bool IsPlayingAudioEvents
        {
            get
            {
                return this.UIComponent.playAudioEvents;
            }
            set
            {
                this.UIComponent.playAudioEvents = value;
            }
        }

        public bool UseBuiltinKeyNavigation
        {
            get
            {
                return this.UIComponent.builtinKeyNavigation;
            }
            set
            {
                this.UIComponent.builtinKeyNavigation = value;
            }
        }

        public virtual bool CanGetFocus
        {
            get
            {
                return this.UIComponent.canFocus;
            }
            set
            {
                this.UIComponent.canFocus = value;
            }
        }

        public int ChildCount
        {
            get
            {
                return this.UIComponent.childCount;
            }
        }

        public IColossalControl[] Children
        {
            get
            {
                return this.UIComponent.components.Select(component =>
                {
                    
                    return new ColossalControl(component);
                }).ToArray();
            }
        }

        public IColossalControl Parent
        {
            get
            {
                UIComponent component = this.UIComponent.parent;

                return new ColossalControl(component);
            }
        }

        public virtual bool ContainsFocus
        {
            get
            {
                return this.UIComponent.containsFocus;
            }
        }

        public bool ContainsMouse
        {
            get
            {
                return this.UIComponent.containsMouse;
            }
        }

        protected bool HasPendingLayoutRequest
        {
            get
            {
                return this.GetUIComponentProperty<bool>("hasPendingLayoutRequest");
            }
        }

        protected bool IsLayoutSuspended
        {
            get
            {
                return this.GetUIComponentProperty<bool>("isLayoutSuspended");
            }
        }

        protected bool IsPerformingLayout
        {
            get
            {
                return this.GetUIComponentProperty<bool>("isPerformingLayout");
            }
        }

        public int TabIndex
        {
            get
            {
                return this.UIComponent.tabIndex;
            }
            set
            {
                this.UIComponent.tabIndex = value;
            }
        }

        public string TooltipLocaleId
        {
            get
            {
                return this.UIComponent.tooltipLocaleID;
            }
            set
            {
                this.UIComponent.tooltipLocaleID = value;
            }
        }

        #endregion

        #region Cached values

        public string CachedName
        {
            get
            {
                return this.UIComponent.cachedName;
            }
            set
            {
                this.UIComponent.cachedName = value;
            }
        }

        public Transform CachedTransform
        {
            get
            {
                return this.UIComponent.cachedTransform;
            }
        }

        #endregion

        public object ObjectUserData
        {
            get
            {
                return this.UIComponent.objectUserData;
            }
            set
            {
                this.UIComponent.objectUserData = value;
            }
        }

        public string StringUserData
        {
            get
            {
                return this.UIComponent.stringUserData;
            }
            set
            {
                this.UIComponent.stringUserData = value;
            }
        }

        protected UIRenderData RenderData
        {
            get
            {
                return this.GetUIComponentProperty<UIRenderData>("renderData");
            }
        }

        public int RenderOrder
        {
            get
            {
                return this.UIComponent.renderOrder;
            }
        }

        #endregion

        #region Methods

        public static ColossalControl FromUIComponent(UIComponent component)
        {
            if(component == null) 
            {
                throw new ArgumentNullException("component");
            }

            return new ColossalControl(component);
        }

        public static TControl FromUIComponent<TControl>(UIComponent component)
            where TControl : ColossalControl
        {
            ColossalControl control = (TControl)FromUIComponent(component);

            return (TControl)Activator.CreateInstance(typeof(TControl), new object[] { control });
        }

        #region Wrapping Colossal UI

        /// <summary>
        /// Initializes the Colossal UI wrapper.
        /// </summary>
        /// <param name="name">The name for the newly created <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="componentType">Type of the wrapped <see cref="ColossalFramework.UI.UIComponent"/>.</param>
        protected virtual void InitializeComponent(string name, Type componentType)
        {
            if (!typeof(ColossalFramework.UI.UIComponent).IsAssignableFrom(componentType))
            {
                throw new InvalidCastException("The given type is not assignable to UIComponent");
            }

            if (ColossalUIView == null)
            {
                Log.Warning("No Colossal UIView found in game.");

                throw new InvalidOperationException("No UIView found in game.");
            }

            this.GameObject = new GameObject(name, componentType);

            this.GameObject.transform.parent = ColossalUIView.transform;

            this.IsActive = false;

            this.UIComponent = (UIComponent)this.GameObject.GetComponent(componentType);
        }

        /// <summary>
        /// Subscribes the internal event wrappers to the events of the wrapped <see cref="UIComponent"/>.
        /// </summary>
        protected virtual void SubscribeEvents()
        {
            this.UIComponent.eventClick += OnClick;
            this.UIComponent.eventAnchorChanged += OnAnchorChanged;
        }

        /// <summary>
        /// Calls a method with the given <paramref name="methodName"/> on the wrapped <see cref="UIComponent"/>.
        /// Use <paramref name="methodParams"/> to specify parameters to be passed to the called method.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the called method.</typeparam>
        /// <param name="methodName">The name of the method to call.</param>
        /// <param name="methodParams">The parameters to pass to the called method.</param>
        /// <returns>The returned value from the called method.</returns>
        protected virtual TResult CallUIComponentMethod<TResult>(string methodName, params object[] methodParams)
        {
            MethodInfo method = this.UIComponent.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                throw new MissingMethodException("Method " + methodName + " not found.");
            }

            object result = method.Invoke(this.UIComponent, methodParams);

            return (TResult)result;
        }

        protected virtual TProp GetUIComponentProperty<TProp>(string propertyName)
        {
            PropertyInfo property = this.UIComponent.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            if (property == null)
            {
                throw new MissingMemberException("Property " + propertyName + " not found.");
            }

            object result = property.GetValue(this.UIComponent, null);

            return (TProp)result;
        }

        protected virtual void SetUIComponentProperty<TProp>(string propertyName, TProp value)
        {
            PropertyInfo property = this.UIComponent.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty);

            if (property == null)
            {
                throw new MissingMemberException("Property " + propertyName + " not found.");
            }

            property.SetValue(this.UIComponent, value, null);
        }

        #endregion

        #region Handling Child Controls

        /// <summary>
        /// Creates a new control of given type <typeparamref name="TControl"/> and attaches it as a child control.
        /// </summary>
        /// <remarks>
        /// This uses the control classes from SkylineToolkit.
        /// </remarks>
        /// <typeparam name="TControl">The type of the new control.</typeparam>
        /// <returns>The newly created control.</returns>
        public TControl AddControl<TControl>()
            where TControl : IColossalControl, new()
        {
            TControl control = new TControl();

            this.UIComponent.AddUIComponent(control.UIComponent.GetType());

            return control;
        }

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
        public IColossalControl AddControl(Type type)
        {
            if (typeof(ColossalControl).IsAssignableFrom(type))
            {
                IColossalControl control = (IColossalControl)Activator.CreateInstance(type);

                this.UIComponent.AddUIComponent(control.UIComponent.GetType());

                return control;
            }

            if (typeof(UIComponent).IsAssignableFrom(type))
            {
                UIComponent component = this.UIComponent.AddUIComponent(type);

                return new ColossalControl(component);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Attaches an already existing control as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <typeparam name="TControl">The type of the existing control.</typeparam>
        /// <param name="control">The existing control which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        public TControl AttachControl<TControl>(TControl control)
            where TControl : IColossalControl, new()
        {
            this.UIComponent.AttachUIComponent(control.UIComponent.gameObject);

            return control;
        }

        /// <summary>
        /// Attaches an already existing control as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <typeparam name="TControl">The type of the existing control.</typeparam>
        /// <param name="control">The existing control which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        public IColossalControl AttachControl(IColossalControl control)
        {
            this.UIComponent.AttachUIComponent(control.UIComponent.gameObject);

            return control;
        }

        /// <summary>
        /// Attaches an already existing UI <paramref name="component"/> as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <remarks>
        /// This wraps the given UI <paramref name="component"/> with the SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="component">The existing UIComponent which should becoma a child control.</param>
        /// <returns>The attached child control.</returns>
        public IColossalControl AttachControl(UIComponent component)
        {
            component = this.UIComponent.AttachUIComponent(component.gameObject);

            return new ColossalControl(component);
        }

        /// <summary>
        /// Attaches an already existing UI component from a given <paramref name="gameObject"/> as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <remarks>
        /// This wraps the given UI <paramref name="gameObject"/> with the SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="gameObject">The gameobject holding the UIComponent which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        public IColossalControl AttachControl(GameObject gameObject)
        {
            UIComponent component = this.UIComponent.AttachUIComponent(gameObject);

            return new ColossalControl(component);
        }

        /// <summary>
        /// Finds a child control matching the given filter.
        /// </summary>
        /// <remarks>
        /// The found <see cref="UIComponent"/>, if there's one, gets automatically wrapped up into a SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="filter">A filter used to search a sub control.</param>
        /// <returns>The found child control or null when found nothin.</returns>
        public IColossalControl FindChild(string filter)
        {
            UIComponent component = this.UIComponent.Find(filter);

            if (component == null)
            {
                return null;
            }

            return new ColossalControl(component);
        }

        #endregion

        #region Positioning and Scaling

        /// <summary>
        /// Aligns the control to the <paramref name="target"/> at the given anchor. 
        /// </summary>
        /// <param name="target">The target control on which to align the control.</param>
        /// <param name="anchor">The anchor on which to align the control.</param>
        public virtual void AlignTo(IColossalControl target, AlignmentAnchor anchor)
        {
            this.UIComponent.AlignTo(target.UIComponent, (UIAlignAnchor)anchor);
        }

        /// <summary>
        /// Aligns the control to the <paramref name="target"/> at the given anchor. 
        /// </summary>
        /// <param name="target">The target control on which to align the control.</param>
        /// <param name="anchor">The anchor on which to align the control.</param>
        public virtual void AlignTo(UIComponent target, AlignmentAnchor anchor)
        {
            this.UIComponent.AlignTo(target, (UIAlignAnchor)anchor);
        }

        /// <summary>
        /// Places the control centered at the center of the <paramref name="target"/> control.
        /// </summary>
        /// <param name="target"></param>
        public void CenterTo(IColossalControl target)
        {
            this.UIComponent.CenterTo(target.UIComponent);
        }

        /// <summary>
        /// Places the control centered at the center of the parent control.
        /// </summary>
        public void CenterToParent()
        {
            this.UIComponent.CenterToParent();
        }

        /// <summary>
        /// Brings the control to the topmost layer of the UI.
        /// </summary>
        public virtual void BringToFront()
        {
            this.UIComponent.BringToFront();
        }

        /// <summary>
        /// Calculates the minimum required size of the control.
        /// </summary>
        /// <returns>The minimum required size of the control.</returns>
        public virtual Vector2 GetMinimumSize()
        {
            return this.UIComponent.CalculateMinimumSize();
        }

        /// <summary>
        /// Calculates the control's current bounds.
        /// </summary>
        /// <returns>The current bounds of the control.</returns>
        public Bounds GetBounds()
        {
            return this.UIComponent.GetBounds();
        }

        public Vector3[] GetCorners()
        {
            return this.UIComponent.GetCorners();
        }

        public void FitToChildren()
        {
            this.UIComponent.FitChildren();
        }

        public void FitToChildren(Vector2 margin)
        {
            this.UIComponent.FitChildren(margin);
        }

        public void FitToChildrenHorizontally()
        {
            this.UIComponent.FitChildrenHorizontally();
        }

        public void FitToChildrenHorizontally(float margin)
        {
            this.UIComponent.FitChildrenHorizontally(margin);
        }

        public void FitToChildrenVertically()
        {
            this.UIComponent.FitChildrenVertically();
        }

        public void FitToChildrenVertically(float margin)
        {
            this.UIComponent.FitChildrenVertically(margin);
        }

        public void FitTo(IColossalControl control)
        {
            this.UIComponent.FitTo(control.UIComponent);
        }

        public void FitTo(IColossalControl control, float margin)
        {
            this.UIComponent.FlexibleFitTo(control.UIComponent, margin);
        }

        public void FitHorizontallyTo(IColossalControl control)
        {
            this.UIComponent.FitTo(control.UIComponent, ColossalFramework.UI.LayoutDirection.Horizontal);
        }

        public void FitVerticallyTo(IColossalControl control)
        {
            this.UIComponent.FitTo(control.UIComponent, ColossalFramework.UI.LayoutDirection.Vertical);
        }

        public bool GetIsClippedFromParent(out ClippingDirections directions)
        {
            ClipDirection clipDirects;

            bool result = this.UIComponent.IsClippedFromParent(out clipDirects);

            directions = (ClippingDirections)clipDirects;

            return result;
        }

        #endregion

        #region Appearance

        /// <summary>
        /// Applies the calculated opacity (opacity of parent elements included) to the given <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The color on which to apply the opacity.</param>
        /// <returns>A new color with the opacity applied.</returns>
        protected virtual Color32 ApplyOpacity(Color32 color)
        {
            return CallUIComponentMethod<Color32>("ApplyOpacity", color);
        }

        /// <summary>
        /// Calculates the opacity for this control including the opacity values of all parent elements.
        /// </summary>
        /// <returns>The calculated opacity for the control.</returns>
        protected virtual float CalculateOpacity()
        {
            return CallUIComponentMethod<float>("CalculateOpacity");
        }

        /// <summary>
        /// Hides the control.
        /// </summary>
        public void Hide()
        {
            this.UIComponent.Hide();
        }

        /// <summary>
        /// Shows the control.
        /// </summary>
        public void Show()
        {
            this.UIComponent.Show();
        }

        #endregion

        public Camera GetCamera()
        {
            return this.UIComponent.GetCamera();
        }

        public Vector3 GetCenter()
        {
            return this.UIComponent.GetCenter();
        }

        protected virtual Plane[] GetClippingPlanes()
        {
            return CallUIComponentMethod<Plane[]>("GetClippingPlanes");
        }

        public bool GetHitPosition(Ray ray, out Vector2 position)
        {
            return this.UIComponent.GetHitPosition(ray, out position);
        }

        protected UITooltipAnchor GetInheritedTooltipAnchor()
        {
            return CallUIComponentMethod<UITooltipAnchor>("GetInheritedTooltipAnchor");
        }

        public IColossalControl GetRootContainer()
        {
            return new ColossalControl(this.UIComponent.GetRootContainer());
        }

        public IColossalControl GetRootCustomContainer()
        {
            return new ColossalControl(this.UIComponent.GetRootCustomControl());
        }

        protected Vector3 GetScaledDirection(Vector3 direction)
        {
            return CallUIComponentMethod<Vector3>("GetScaledDirection", direction);
        }

        // TODO: Wrapper for UIView ?
        /// <summary>
        /// Searches the next <see cref="UIView"/> up in the UI hierarchy.
        /// </summary>
        /// <returns>The found <see cref="UIView"/>.</returns>
        public UIView GetUIView()
        {
            return this.GetUIView();
        }

        public virtual void Invalidate()
        {
            this.UIComponent.Invalidate();
        }

        #region Event handling

        protected bool InvokeEvent(string eventName, params object[] args)
        {
            return CallUIComponentMethod<bool>("Invoke", eventName, args);
        }

        protected bool InvokeEvent(GameObject target, string eventName, params object[] args)
        {
            return CallUIComponentMethod<bool>("Invoke", eventName, args);
        }

        protected bool InvokeEventUpward(string eventName, params object[] args)
        {
            return CallUIComponentMethod<bool>("InvokeUpward", eventName, args);
        }

        #endregion

        public virtual void Localize()
        {
            this.UIComponent.Localize();
        }

        public virtual void LocalizeTooltip()
        {
            this.UIComponent.LocalizeTooltip();
        }

        public void MakePixelPerfect()
        {
            this.MakePixelPerfect(true);
        }

        public void MakePixelPerfect(bool recursive)
        {
            this.UIComponent.MakePixelPerfect(recursive);
        }

        public virtual void MoveBackward()
        {
            this.UIComponent.MoveBackward();
        }

        public virtual void MoveForward()
        {
            this.UIComponent.MoveForward();
        }

        public void PerformLayout()
        {
            this.UIComponent.PerformLayout();
        }

        protected float PixelsToUnits()
        {
            return this.CallUIComponentMethod<float>("PixelsToUnits");
        }

        protected void PlayClickSound()
        {
            this.CallUIComponentMethod<object>("PlayClickSound", this.UIComponent);
        }

        protected void PlayDisabledClickSound()
        {
            this.CallUIComponentMethod<object>("PlayDisabledClickSound", this.UIComponent);
        }

        public bool Raycast(Ray ray)
        {
            Vector3 hitPoint;

            return this.Raycast(ray, out hitPoint);
        }

        public bool Raycast(Ray ray, out Vector3 hitPoint)
        {
            return this.UIComponent.Raycast(ray, out hitPoint);
        }

        public HitPointInfo[] RaycastAll(Ray ray)
        {
            List<UIHitInfo> hitInfos = new List<UIHitInfo>();

            this.UIComponent.Raycast(ray, hitInfos);

            return hitInfos.Select(h => new HitPointInfo(h)).ToArray();
        }

        public void RefreshTooltip()
        {
            this.UIComponent.RefreshTooltip();
        }

        protected void RemoveAllEventHandlers()
        {
            foreach (FieldInfo fieldInfo in GetAllEventFields(this.GetType()))
            {
                fieldInfo.SetValue((object)this, (object)null);
            }

            this.CallUIComponentMethod<object>("RemoveAllEventHandlers");
        }

        public void RemoveControl<TControl>(TControl control)
            where TControl : IColossalControl
        {
            this.UIComponent.RemoveUIComponent(control.UIComponent);
        }

        public virtual void ResetLayout()
        {
            this.UIComponent.ResetLayout();
        }

        public virtual void ResetLayout(bool recursive, bool force)
        {
            this.UIComponent.ResetLayout(recursive, force);
        }

        public virtual void SendToBack()
        {
            this.UIComponent.SendToBack();
        }

        public void Show(bool bringToFront)
        {
            this.UIComponent.Show(bringToFront);
        }

        public void SimulateClick()
        {
            this.UIComponent.SimulateClick();
        }

        protected Vector3 TransformOffset(Vector3 offset)
        {
            return this.CallUIComponentMethod<Vector3>("TransformOffset", offset);
        }

        private static FieldInfo[] GetAllEventFields(Type type)
        {
            if (type == null)
            {
                return new FieldInfo[0];
            }

            BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var fieldInfos = type.GetFields(bindingFlags).Where(fi => typeof(Delegate).IsAssignableFrom(fi.FieldType) && !fi.IsDefined(typeof(HideInInspector), true));

            return fieldInfos.Concat(GetAllEventFields(type.BaseType)).ToArray();
        }

        #endregion

        #region Operators

        public static implicit operator UIComponent(ColossalControl control)
        {
            return control.UIComponent;
        }

        public static ColossalControl operator +(ColossalControl control1, IColossalControl control2)
        {
            control1.AttachControl(control2);

            return control1;
        }

        #endregion

        #region Event wrappers

        #region Mouse actions

        protected void OnClick(UIComponent component, UIMouseEventParameter e)
        {
            if (this.Click != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.Click(this, args);
            }
        }

        protected void OnDisabledClick(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DisabledClick != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DisabledClick(this, args);
            }
        }

        protected void OnDoubleClick(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DoubleClick != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DoubleClick(this, args);
            }
        }

        protected void OnMouseDown(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseDown != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseDown(this, args);
            }
        }

        protected void OnMouseUp(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseUp != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseUp(this, args);
            }
        }

        protected void OnMouseWheel(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseWheel != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseWheel(this, args);
            }
        }

        protected void OnMouseEnter(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseEnter != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseEnter(this, args);
            }
        }

        protected void OnMouseLeave(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseLeave != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseLeave(this, args);
            }
        }

        protected void OnMouseHover(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseHover != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseHover(this, args);
            }
        }

        protected void OnMouseMove(UIComponent component, UIMouseEventParameter e)
        {
            if (this.MouseMove != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.MouseMove(this, args);
            }
        }

        protected void OnDragStart(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragStart != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragStart(this, args);
            }
        }

        protected void OnDragEnd(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragEnd != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragEnd(this, args);
            }
        }

        protected void OnDragEnter(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragEnter != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragEnter(this, args);
            }
        }

        protected void OnDragLeave(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragLeave != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragLeave(this, args);
            }
        }

        protected void OnDragOver(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragOver != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragOver(this, args);
            }
        }

        protected void OnDragDrop(UIComponent component, UIMouseEventParameter e)
        {
            if (this.DragDrop != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.DragDrop(this, args);
            }
        }

        protected void OnTooltipEnter(UIComponent component, UIMouseEventParameter e)
        {
            if (this.TooltipEnter != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.TooltipEnter(this, args);
            }
        }

        protected void OnTooltipLeave(UIComponent component, UIMouseEventParameter e)
        {
            if (this.TooltipLeave != null)
            {
                MouseEventArgs args = new MouseEventArgs(e);

                this.TooltipLeave(this, args);
            }
        }

        #endregion

        #region Keyboard actions

        protected void OnKeyDown(UIComponent component, UIKeyEventParameter e)
        {
            if (this.KeyDown != null)
            {
                KeyboardEventArgs args = new KeyboardEventArgs(e);

                this.KeyDown(this, args);
            }
        }

        protected void OnKeyUp(UIComponent component, UIKeyEventParameter e)
        {
            if (this.KeyUp != null)
            {
                KeyboardEventArgs args = new KeyboardEventArgs(e);

                this.KeyUp(this, args);
            }
        }

        protected void OnKeyPress(UIComponent component, UIKeyEventParameter e)
        {
            if (this.KeyPress != null)
            {
                KeyboardEventArgs args = new KeyboardEventArgs(e);

                this.KeyPress(this, args);
            }
        }

        #endregion

        #region Child controls

        protected void OnChildControlAdded(UIComponent component, UIComponent e)
        {
            if (this.ChildControlAdded != null)
            {
                this.ChildControlAdded(this, FromUIComponent(e));
            }
        }

        protected void OnChildControlRemoved(UIComponent component, UIComponent e)
        {
            if (this.ChildControlRemoved != null)
            {
                this.ChildControlRemoved(this, FromUIComponent(e));
            }
        }

        #endregion

        #region Positioning and Scaling

        protected void OnAnchorChanged(UIComponent component, UIAnchorStyle e)
        {
            if (this.AnchorChanged != null)
            {
                PropChangedEventArgs<PositionAnchor> args = new PropChangedEventArgs<PositionAnchor>("Anchor", (PositionAnchor)e);

                this.AnchorChanged(this, args);
            }
        }

        protected void OnFitChildren()
        {
            if (this.FitChildren != null)
            {
                this.FitChildren(this);
            }
        }

        protected void OnPositionChanged(UIComponent component, Vector2 e)
        {
            if (this.PositionChanged != null)
            {
                PropChangedEventArgs<Vector2> args = new PropChangedEventArgs<Vector2>("Position", e);

                this.PositionChanged(this, args);
            }
        }

        protected void OnSizeChanged(UIComponent component, Vector2 e)
        {
            if (this.SizeChanged != null)
            {
                PropChangedEventArgs<Vector2> args = new PropChangedEventArgs<Vector2>("Size", e);

                this.SizeChanged(this, args);
            }
        }

        #endregion

        #region Appearance

        protected void OnColorChanged(UIComponent component, Color32 e)
        {
            if (this.ColorChanged != null)
            {
                PropChangedEventArgs<Color32> args = new PropChangedEventArgs<Color32>("Color", e);

                this.ColorChanged(this, args);
            }
        }

        protected void OnDisabledColorChanged(UIComponent component, Color32 e)
        {
            if (this.DisabledColorChanged != null)
            {
                PropChangedEventArgs<Color32> args = new PropChangedEventArgs<Color32>("DisabledColor", e);

                this.DisabledColorChanged(this, args);
            }
        }

        protected void OnOpacityChanged(UIComponent component, float e)
        {
            if (this.OpacityChanged != null)
            {
                PropChangedEventArgs<float> args = new PropChangedEventArgs<float>("Opacity", e);

                this.OpacityChanged(this, args);
            }
        }

        protected void OnPivotChanged(UIComponent component, PivotPoint e)
        {
            if (this.PivotChanged != null)
            {
                PropChangedEventArgs<PivotPoint> args = new PropChangedEventArgs<PivotPoint>("Pivot", e);
                
                this.PivotChanged(this, args);
            }
        }

        protected void OnTooltipShow(UIComponent component, UITooltipEventParameter e)
        {
            if (this.TooltipShow != null)
            {
                TooltipEventArgs args = new TooltipEventArgs(e);

                this.TooltipShow(this, args);
            }
        }

        protected void OnTooltipHide(UIComponent component, UITooltipEventParameter e)
        {
            if (this.TooltipHide != null)
            {
                TooltipEventArgs args = new TooltipEventArgs(e);

                this.TooltipHide(this, args);
            }
        }

        protected void OnTooltipTextChanged(UIComponent component, string e)
        {
            if (this.TooltipTextChanged != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("TooltipText", e);
                
                this.TooltipTextChanged(this, args);
            }
        }

        protected void OnZOrderChanged(UIComponent component, int e)
        {
            if (this.ZOrderChanged != null)
            {
                PropChangedEventArgs<int> args = new PropChangedEventArgs<int>("ZOrder", e);

                this.ZOrderChanged(this, args);
            }
        }

        #endregion

        #region State

        protected void OnFocus(UIComponent component, UIFocusEventParameter e)
        {
            if (this.Focus != null)
            {
                FocusEventArgs args = new FocusEventArgs(e);

                this.Focus(this, args);
            }
        }

        protected void OnFocused(UIComponent component, UIFocusEventParameter e)
        {
            if (this.Focused != null)
            {
                FocusEventArgs args = new FocusEventArgs(e);

                this.Focused(this, args);
            }
        }

        protected void OnUnfocus(UIComponent component, UIFocusEventParameter e)
        {
            if (this.Unfocus != null)
            {
                FocusEventArgs args = new FocusEventArgs(e);

                this.Unfocus(this, args);
            }
        }

        protected void OnUnfocused(UIComponent component, UIFocusEventParameter e)
        {
            if (this.Unfocused != null)
            {
                FocusEventArgs args = new FocusEventArgs(e);

                this.Unfocused(this, args);
            }
        }

        protected void OnIsEnabledChanged(UIComponent component, bool e)
        {
            if (e)
            {
                if (this.Enabled != null)
                {
                    PropChangedEventArgs<bool> args = new PropChangedEventArgs<bool>("IsEnabled", e);

                    this.Enabled(this, args);
                }
            }
            else
            {
                if (this.Disabled != null)
                {
                    PropChangedEventArgs<bool> args = new PropChangedEventArgs<bool>("IsEnabled", e);

                    this.Disabled(this, args);
                }
            }
        }

        protected void OnVisibilityChanged(UIComponent component, bool e)
        {
            if (this.VisibilityChanged != null)
            {
                PropChangedEventArgs<bool> args = new PropChangedEventArgs<bool>("Color", e);

                this.VisibilityChanged(this, args);
            }
        }

        protected void OnTabIndexChanged(UIComponent component, int e)
        {
            if (this.TabIndexChanged != null)
            {
                PropChangedEventArgs<int> args = new PropChangedEventArgs<int>("Color", e);

                this.TabIndexChanged(this, args);
            }
        }

        #endregion

        #endregion

        #region IComparable<ColossalControl<T>>

        public int CompareTo(IColossalControl other)
        {
            return this.UIComponent.CompareTo(other.UIComponent);
        }

        #endregion
    }
}
