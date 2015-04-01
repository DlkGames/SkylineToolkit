using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using UnityEngine;
namespace SkylineToolkit.UI
{
    public interface IColossalControl : IControl
    {
        #region Events

        #region Mouse actions

        event MouseEventHandler Click;

        event MouseEventHandler DisabledClick;

        event MouseEventHandler DoubleClick;

        event MouseEventHandler MouseDown;

        event MouseEventHandler MouseUp;

        event MouseEventHandler MouseWheel;

        event MouseEventHandler MouseEnter;

        event MouseEventHandler MouseLeave;

        event MouseEventHandler MouseHover;

        event MouseEventHandler MouseMove;

        event MouseEventHandler DragStart;

        event MouseEventHandler DragEnd;

        event MouseEventHandler DragEnter;

        event MouseEventHandler DragLeave;

        event MouseEventHandler DragOver;

        event MouseEventHandler DragDrop;

        event MouseEventHandler TooltipEnter;

        event MouseEventHandler TooltipLeave;

        #endregion

        #region Keyboard actions

        event KeyboardEventHandler KeyDown;

        event KeyboardEventHandler KeyUp;

        event KeyboardEventHandler KeyPress;

        #endregion

        #region Child controls

        event ChildControlEventHandler ChildControlAdded;

        event ChildControlEventHandler ChildControlRemoved;

        #endregion

        #region Positioning and Scaling

        event PropChangedEventHandler<PositionAnchor> AnchorChanged;

        event FitChildrenEventHandler FitChildren;

        event PropChangedEventHandler<Vector2> PositionChanged;

        event PropChangedEventHandler<Vector2> SizeChanged;

        #endregion

        #region Appearance

        event PropChangedEventHandler<Color32> ColorChanged;

        event PropChangedEventHandler<Color32> DisabledColorChanged;

        event PropChangedEventHandler<float> OpacityChanged;

        event PropChangedEventHandler<PivotPoint> PivotChanged;

        event TooltipEventHandler TooltipShow;

        event TooltipEventHandler TooltipHide;

        event PropChangedEventHandler<string> TooltipTextChanged;

        event PropChangedEventHandler<int> ZOrderChanged;

        #endregion

        #region State

        event FocusEventHandler Focus;

        event FocusEventHandler Focused;

        event FocusEventHandler Unfocus;

        event FocusEventHandler Unfocused;

        event PropChangedEventHandler<bool> Enabled;

        event PropChangedEventHandler<bool> Disabled;

        event PropChangedEventHandler<bool> VisibilityChanged;

        event PropChangedEventHandler<int> TabIndexChanged;

        #endregion

        #endregion

        #region Properties

        #region Component

        UIComponent UIComponent { get; }

        string Name { get; set; }

        #endregion

        #region Position and Scaling

        PositionAnchor Anchor { get; set; }

        Vector3 ArbitaryPivotOffset { get; set; }

        Vector4 Area { get; set; }

        Vector2 MaximumSize { get; set; }

        Vector2 MinimumSize { get; set; }

        Vector3 Position { get; set; }

        Vector3 AbsolutePosition { get; set; }

        Vector3 RelativePosition { get; set; }

        bool IsClippedFromParent { get; }

        bool IsAutoSize { get; set; }

        Vector3 Center { get; }

        bool IsClippingChildren { get; set; }

        float Height { get; set; }

        float Width { get; set; }

        Vector2 Size { get; set; }

        int ZOrder { get; set; }

        #endregion

        #region Appearance

        AudioClip ClickSound { get; set; }

        AudioClip DisabledClickSound { get; set; }

        Color32 Color { get; set; }

        Color32 DisabledColor { get; set; }

        CursorInfo HoverCursorStyle { get; set; }

        float Opacity { get; set; }

        PivotPoint Pivot { get; set; }

        string TooltipText { get; set; }

        TooltipAnchor TooltipAnchor { get; set; }

        IColossalControl TooltipBox { get; set; }

        #endregion

        #region State

        bool IsEnabled { get; set; }

        bool HasFocus { get; set; }

        bool IsVisible { get; set; }

        bool IsVisibleSelf { get; }

        bool IsVisibleInParent { get; }

        bool IsInteractive { get; set; }

        bool IsTooltipOnTop { get; set; }

        bool IsLocalized { get; set; }

        bool IsTooltipLocalized { get; set; }

        bool IsPlayingAudioEvents { get; set; }

        bool UseBuiltinKeyNavigation { get; set; }

        bool CanGetFocus { get; set; }

        int ChildCount { get; }

        IColossalControl[] Children { get; }

        IColossalControl Parent { get; }

        bool ContainsFocus { get; }

        bool ContainsMouse { get; }

        int TabIndex { get; set; }

        string TooltipLocaleId { get; set; }

        #endregion

        #region Cached Values

        string CachedName { get; set; }

        Transform CachedTransform { get; }

        #endregion

        object ObjectUserData { get; set; }

        string StringUserData { get; set; }

        int RenderOrder { get; }

        #endregion

        #region Methods

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
        IColossalControl AddControl(Type type);

        /// <summary>
        /// Creates a new control of given type <typeparamref name="TControl"/> and attaches it as a child control.
        /// </summary>
        /// <remarks>
        /// This uses the control classes from SkylineToolkit.
        /// </remarks>
        /// <typeparam name="TControl">The type of the new control.</typeparam>
        /// <returns>The newly created control.</returns>
        TControl AddControl<TControl>() where TControl : IColossalControl, new();

        /// <summary>
        /// Attaches an already existing UI <paramref name="component"/> as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <remarks>
        /// This wraps the given UI <paramref name="component"/> with the SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="component">The existing UIComponent which should becoma a child control.</param>
        /// <returns>The attached child control.</returns>
        IColossalControl AttachControl(UIComponent component);

        /// <summary>
        /// Attaches an already existing control as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <typeparam name="TControl">The type of the existing control.</typeparam>
        /// <param name="control">The existing control which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        IColossalControl AttachControl(IColossalControl control);

        /// <summary>
        /// Attaches an already existing UI component from a given <paramref name="gameObject"/> as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <remarks>
        /// This wraps the given UI <paramref name="gameObject"/> with the SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="gameObject">The gameobject holding the UIComponent which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        IColossalControl AttachControl(GameObject gameObject);

        /// <summary>
        /// Attaches an already existing control as a sub control. Moves the existing control as a child below this control.
        /// </summary>
        /// <typeparam name="TControl">The type of the existing control.</typeparam>
        /// <param name="control">The existing control which should become a child control.</param>
        /// <returns>The attached child control.</returns>
        TControl AttachControl<TControl>(TControl control) where TControl : IColossalControl;

        /// <summary>
        /// Finds a child control matching the given filter.
        /// </summary>
        /// <remarks>
        /// The found <see cref="UIComponent"/>, if there's one, gets automatically wrapped up into a SkylineToolkit control wrapper.
        /// </remarks>
        /// <param name="filter">A filter used to search a sub control.</param>
        /// <returns>The found child control or null when found nothin.</returns>
        IColossalControl FindChild(string filter);

        #endregion

        #region Position and Scaling

        /// <summary>
        /// Aligns the control to the <paramref name="target"/> at the given anchor. 
        /// </summary>
        /// <param name="target">The target control on which to align the control.</param>
        /// <param name="anchor">The anchor on which to align the control.</param>
        void AlignTo(UIComponent target, AlignmentAnchor anchor);

        /// <summary>
        /// Aligns the control to the <paramref name="target"/> at the given anchor. 
        /// </summary>
        /// <param name="target">The target control on which to align the control.</param>
        /// <param name="anchor">The anchor on which to align the control.</param>
        void AlignTo(IColossalControl target, AlignmentAnchor anchor);

        /// <summary>
        /// Places the control centered at the center of the <paramref name="target"/> control.
        /// </summary>
        /// <param name="target"></param>
        void CenterTo(IColossalControl target);

        /// <summary>
        /// Places the control centered at the center of the parent control.
        /// </summary>
        void CenterToParent();

        /// <summary>
        /// Brings the control to the topmost layer of the UI.
        /// </summary>
        void BringToFront();

        /// <summary>
        /// Calculates the minimum required size of the control.
        /// </summary>
        /// <returns>The minimum required size of the control.</returns>
        Vector2 GetMinimumSize();

        /// <summary>
        /// Calculates the control's current bounds.
        /// </summary>
        /// <returns>The current bounds of the control.</returns>
        Bounds GetBounds();

        Vector3[] GetCorners();

        void FitToChildren();

        void FitToChildren(Vector2 margin);

        void FitToChildrenHorizontally();

        void FitToChildrenHorizontally(float margin);

        void FitToChildrenVertically();

        void FitToChildrenVertically(float margin);

        void FitTo(IColossalControl control);

        void FitTo(IColossalControl control, float margin);

        void FitHorizontallyTo(IColossalControl control);

        void FitVerticallyTo(IColossalControl control);

        bool GetIsClippedFromParent(out ClippingDirections directions);

        #endregion

        #region Appearance

        /// <summary>
        /// Hides the control.
        /// </summary>
        void Hide();

        /// <summary>
        /// Shows the control.
        /// </summary>
        void Show();

        void Show(bool bringToFront);

        #endregion

        Camera GetCamera();

        Vector3 GetCenter();

        bool GetHitPosition(Ray ray, out Vector2 position);

        IColossalControl GetRootContainer();

        IColossalControl GetRootCustomContainer();

        UIView GetUIView();

        int CompareTo(IColossalControl other);

        void Invalidate();

        Vector4 Limits { get; set; }

        void Localize();

        void LocalizeTooltip();

        void MakePixelPerfect();

        void MakePixelPerfect(bool recursive);

        void MoveBackward();

        void MoveForward();

        void PerformLayout();

        bool Raycast(Ray ray);

        bool Raycast(Ray ray, out Vector3 hitPoint);

        HitPointInfo[] RaycastAll(Ray ray);

        void RefreshTooltip();

        void RemoveControl<TControl>(TControl control) where TControl : IColossalControl;

        void ResetLayout();

        void ResetLayout(bool recursive, bool force);

        void SendToBack();

        void SimulateClick();

        #endregion
    }
}
