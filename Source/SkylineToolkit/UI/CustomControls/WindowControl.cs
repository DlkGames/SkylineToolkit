using ColossalFramework.UI;
using SkylineToolkit.Events;
using SkylineToolkit.UI.ActionContorls;
using SkylineToolkit.UI.Generic;
using System;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class WindowControl : CustomControl
    {
        private Panel windowPanel;

        private Panel innerPanel;

        private DragHandle dragHandle;

        private ColossalControl<UISlicedSprite> captionComponent;

        private ColossalControl<UILabel> labelComponent;

        private Button closeButton;

        private Button resizeButton;

        private bool isVisible = true;

        private bool isResizable = false;

        private bool isResizing = false;

        private Vector3 cachedPosition;
        private Vector3 cachedLastPosition;
        private Anchor cachedAnchor;

        #region Events

        public event EventHandler<CancellableEventArgs> Close;

        public event EventHandler Closed;

        public event EventHandler<CancellableEventArgs> Open;

        public event EventHandler Opened;

        public event EventHandler Resize;

        public event EventHandler Resizing;

        public event EventHandler SizeChanged;

        #endregion

        #region Properties

        #region Controls

        public DragHandle DragHandle
        {
            get { return dragHandle; }
        }

        public ColossalControl<UILabel> LabelComponent
        {
            get { return labelComponent; }
        }

        public ColossalControl<UISlicedSprite> CaptionComponent
        {
            get { return captionComponent; }
        }

        public Button CloseButton
        {
            get { return closeButton; }
        }

        public Button ResizeButton
        {
            get { return resizeButton; }
        }

        #endregion

        public string Title
        {
            get
            {
                return this.labelComponent.UIComponent.text;
            }
            set
            {
                this.labelComponent.UIComponent.text = value;
            }
        }

        public Vector2 Position
        {
            get { return this.windowPanel.RelativePosition; }
            set { this.windowPanel.RelativePosition = value; }
        }

        public Vector2 Size
        {
            get { return this.windowPanel.Size; }
            set { this.windowPanel.Size = value; }
        }

        public Vector2 MinimumSize
        {
            get { return this.windowPanel.MinSize; }
            set { this.windowPanel.MinSize = value; }
        }

        public Vector2 MaximumSize
        {
            get { return this.windowPanel.MaxSize; }
            set { this.windowPanel.MaxSize = value; }
        }

        public Anchor Anchor
        {
            get { return this.windowPanel.Anchor; }
            set { this.windowPanel.Anchor = value; }
        }

        public Panel WindowPanel
        {
            get { return windowPanel; }
        }

        public Panel InnerPanel
        {
            get { return innerPanel; }
        }

        public RectOffset Padding
        {
            get { return this.innerPanel.Padding; }
            set { this.innerPanel.Padding = value; }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (value)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }
            }
        }

        public bool IsResizable
        {
            get { return isResizable; }
            set 
            { 
                isResizable = value;

                if (resizeButton != null)
                {
                    resizeButton.IsEnabled = value;
                }
            }
        }

        #endregion

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnEnable()
        {
            isResizing = false;
        }

        protected virtual void OnDisable()
        {
            this.Hide();

            isResizing = false;
        }

        public virtual void Hide()
        {
            if (!isVisible)
            {
                return;
            }

            CancellableEventArgs args = new CancellableEventArgs();

            this.OnClose(args);

            if (args.Cancel)
            {
                return;
            }

            this.StopResizing();
            this.windowPanel.Hide();
            this.isVisible = false;

            this.OnClosed();
        }

        public virtual void Show()
        {
            if (this.isVisible)
            {
                return;
            }

            CancellableEventArgs args = new CancellableEventArgs();

            this.OnOpen(args);

            if (args.Cancel)
            {
                return;
            }

            this.windowPanel.Show();
            this.isVisible = true;

            this.OnOpened();
        }

        protected virtual void Initialize()
        {
            CreateWindowPanel();

            CreateTitlebar();

            CreateResizeHandle();

            SubscribeEvents();

            windowPanel.IsActive = true;
        }

        protected virtual void CreateWindowPanel()
        {
            windowPanel = new Panel(this.gameObject.AddComponent<UIPanel>(), true);
            windowPanel.Anchor = Anchor.CenterHorizontal | Anchor.CenterVertical;
            windowPanel.IsInteractive = true;
            windowPanel.CanGetFocus = true;
            windowPanel.Pivot = PivotPoint.TopLeft;
            windowPanel.BackgroundSprite = "MenuPanel";
            windowPanel.MinSize = new Vector2(300, 200);
            windowPanel.MaxSize = new Vector2(8000, 8000);

            innerPanel = new Panel("InnerPanel");
            windowPanel.AttachControl(innerPanel);
            innerPanel.Anchor = Anchor.All;
            innerPanel.ZOrder = 0;
            innerPanel.IsActive = true;
            innerPanel.RelativePosition = new Vector3(5, 45);
            innerPanel.Size = new Vector2(windowPanel.Width - 10, windowPanel.Height - 50);
            innerPanel.BackgroundSprite = String.Empty;
        }

        protected virtual void CreateTitlebar()
        {
            captionComponent = new ColossalControl<UISlicedSprite>("Caption");
            windowPanel.AttachControl(captionComponent);
            captionComponent.Width = windowPanel.Width;
            captionComponent.Height = 40;
            captionComponent.Anchor = Anchor.Top | Anchor.Left | Anchor.Right;
            captionComponent.ZOrder = 0;
            captionComponent.IsActive = true;
            captionComponent.RelativePosition = Vector3.zero;

            dragHandle = new DragHandle("Drag Handle");
            captionComponent.AttachControl(DragHandle);
            dragHandle.Target = windowPanel;
            dragHandle.Width = windowPanel.Width;
            dragHandle.Height = 40;
            dragHandle.Anchor = Anchor.All;
            dragHandle.ZOrder = 1;
            dragHandle.IsActive = true;
            dragHandle.RelativePosition = Vector3.zero;

            labelComponent = new ColossalControl<UILabel>("Label");
            captionComponent.AttachControl(labelComponent);
            labelComponent.Height = 40;
            labelComponent.Anchor = Anchor.CenterHorizontal | Anchor.CenterVertical;
            labelComponent.EnableAutoSize = true;
            labelComponent.Color = new Color32(254, 254, 254, 255);
            labelComponent.UIComponent.textColor = new Color32(254, 254, 254, 255);
            labelComponent.Pivot = PivotPoint.TopLeft;
            labelComponent.ZOrder = 0;
            labelComponent.IsActive = true;

            closeButton = new Button("Close");
            captionComponent.AttachControl(closeButton);
            closeButton.Text = String.Empty;
            closeButton.IsTooltipOnTop = true;
            closeButton.CharacterSpacing = 0;
            closeButton.Color = new Color32(254, 254, 254, 255);
            closeButton.Width = 32;
            closeButton.Height = 32;
            closeButton.NormalBackgroundSprite = "buttonclose";
            closeButton.HoveredColor = new Color32(254, 254, 254, 255);
            closeButton.HoveredBackgroundSprite = "buttonclosehover";
            closeButton.PressedColor = new Color32(254, 254, 254, 255);
            closeButton.PressedBackgroundSprite = "buttonclosepressed";
            closeButton.FocusedColor = new Color32(254, 254, 254, 255);
            closeButton.FocusedBackgroundSprite = "buttonclose";
            closeButton.OutlineColor = new Color32(0, 0, 0, 255);
            closeButton.Pivot = PivotPoint.TopLeft;
            closeButton.Anchor = Anchor.Top | Anchor.Right;
            closeButton.ZOrder = 2;
            closeButton.IsActive = true;
            closeButton.RelativePosition = new Vector3(this.windowPanel.Width - 36, 4);
        }

        protected virtual void CreateResizeHandle()
        {
            resizeButton = new Button("Resize");
            windowPanel.AttachControl(resizeButton);
            resizeButton.Text = String.Empty;
            resizeButton.IsTooltipOnTop = true;
            resizeButton.CharacterSpacing = 0;
            resizeButton.Color = new Color32(254, 254, 254, 255);
            resizeButton.Width = 24;
            resizeButton.Height = 24;
            resizeButton.NormalBackgroundSprite = "buttonresize";
            resizeButton.HoveredColor = new Color32(254, 254, 254, 255);
            resizeButton.HoveredBackgroundSprite = "buttonresize";
            resizeButton.PressedColor = new Color32(254, 254, 254, 255);
            resizeButton.PressedBackgroundSprite = "buttonclose";
            resizeButton.FocusedColor = new Color32(254, 254, 254, 255);
            resizeButton.FocusedBackgroundSprite = "buttonresize";
            resizeButton.OutlineColor = new Color32(0, 0, 0, 255);
            resizeButton.Pivot = PivotPoint.TopLeft;
            resizeButton.Anchor = Anchor.Bottom | Anchor.Right;
            resizeButton.IsActive = true;
            resizeButton.IsEnabled = this.isResizable;
            resizeButton.RelativePosition = new Vector3(windowPanel.Width - 28, windowPanel.Height - 28);
            resizeButton.ZOrder = 2;
        }

        protected void SubscribeEvents()
        {
            closeButton.Click += closeButton_Click;

            resizeButton.MouseDown += resizeButton_MouseDown;
            resizeButton.MouseUp += resizeButton_MouseUp;
            resizeButton.MouseMove += resizeButton_MouseMove;
        }

        protected void UnsubscribeEvents()
        {
            closeButton.Click -= closeButton_Click;

            resizeButton.MouseDown -= resizeButton_MouseDown;
            resizeButton.MouseUp -= resizeButton_MouseUp;
            resizeButton.MouseMove -= resizeButton_MouseMove;
        }

        #region Event handlers

        private void closeButton_Click(object sender, MouseEventArgs e)
        {
            this.Hide();
        }

        private void resizeButton_MouseDown(object sender, MouseEventArgs e)
        {
            StartResizing(e);
        }

        private void resizeButton_MouseMove(object sender, MouseEventArgs e)
        {
            DoResizing(e);
        }

        private void resizeButton_MouseUp(object sender, MouseEventArgs e)
        {
            StopResizing();
        }

        #endregion

        #region Methods

        #region Resizing

        protected virtual void StartResizing(MouseEventArgs e)
        {
            Log.Verbose("Start resizing window");

            windowPanel.BringToFront();

            isResizing = true;

            this.cachedAnchor = this.windowPanel.Anchor;
            this.cachedPosition = this.windowPanel.AbsolutePosition;
            this.windowPanel.Anchor = Anchor.Top | Anchor.Left;


            e.Handled = true;

            Ray ray = e.Ray;
            float enter = 0.0f;
            Plane detectionPlane = new Plane(windowPanel.GameObject.transform.TransformDirection(Vector3.back), windowPanel.GameObject.transform.position);
            detectionPlane.Raycast(ray, out enter);

            this.cachedLastPosition = ray.origin + ray.direction * enter;

            e.Handled = true;

            this.OnResize();
        }

        protected virtual void DoResizing(MouseEventArgs e)
        {
            if (isResizing)
            {
                if (e.CheckButtonsPressed(MouseButtons.Left))
                {
                    e.Handled = true;

                    Ray ray = e.Ray;
                    float enter = 0.0f;

                    Plane detectionPlane = new Plane(windowPanel.GetUIView().uiCamera.transform.TransformDirection(Vector3.back), this.cachedLastPosition);
                    detectionPlane.Raycast(ray, out enter);

                    Vector3 mousePositionW = VectorExtensions.Quantize(ray.origin + ray.direction * enter, windowPanel.PixelsToUnits());

                    Vector3 transformVecToTopLeftW = UIPivotExtensions.TransformToUpperLeft((UIPivotPoint)windowPanel.Pivot, windowPanel.Size, windowPanel.ArbitaryPivotOffset);

                    Vector3 point1S = windowPanel.GameObject.transform.position + transformVecToTopLeftW;

                    Vector2 newSize = (mousePositionW - point1S) / windowPanel.PixelsToUnits();
                    newSize = new Vector2(newSize.x, -newSize.y);

                    if (newSize.x < this.windowPanel.MinSize.x) newSize.x = this.windowPanel.MinSize.x;
                    else if (newSize.x > this.windowPanel.MaxSize.x) newSize.x = this.windowPanel.MaxSize.x;
                    if (newSize.y < this.windowPanel.MinSize.y) newSize.y = this.windowPanel.MinSize.y;
                    else if (newSize.y > this.windowPanel.MaxSize.y) newSize.y = this.windowPanel.MaxSize.y;

                    windowPanel.Size = newSize;

                    this.cachedLastPosition = mousePositionW;

                    this.OnResizing();
                }
                else
                {
                    StopResizing();
                }
            }
        }

        protected virtual void StopResizing()
        {
            if (!isResizing)
            {
                return;
            }

            Log.Verbose("Stop resizing window");

            isResizing = false;
            windowPanel.Anchor = this.cachedAnchor;
            windowPanel.AbsolutePosition = this.cachedPosition;

            windowPanel.MakePixelPerfect();

            this.OnSizeChanged();
        }

        #endregion

        #endregion

        #region Event triggers

        protected virtual void OnClose(CancellableEventArgs args)
        {
            if (this.Close != null)
            {
                this.Close(this, args);
            }
        }

        protected virtual void OnClosed()
        {
            if (this.Closed != null)
            {
                this.Closed(this, null);
            }
        }

        protected virtual void OnOpen(CancellableEventArgs args)
        {
            if (this.Open != null)
            {
                this.Open(this, args);
            }
        }

        protected virtual void OnOpened()
        {
            if (this.Opened != null)
            {
                this.Opened(this, null);
            }
        }

        /// <summary>
        /// TODO: EventArgs
        /// </summary>
        protected virtual void OnResize()
        {
            if (this.Resize != null)
            {
                this.Resize(this, null);
            }
        }

        /// <summary>
        /// TODO: EventArgs
        /// </summary>
        protected virtual void OnResizing()
        {
            if (this.Resizing != null)
            {
                this.Resizing(this, null);
            }
        }

        /// <summary>
        /// TODO: EventArgs
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, null);
            }
        }

        #endregion
        
        ~WindowControl()
        {
            this.Dispose(true);
        }

        #region IDisposable

        protected void DisposeComponents()
        {
            windowPanel.Dispose();
            innerPanel.Dispose();
            dragHandle.Dispose();
            captionComponent.Dispose();
            labelComponent.Dispose();
            closeButton.Dispose();
            resizeButton.Dispose();
        }

        public override void Dispose()
        {
            this.Dispose(true);
        }

        bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                this.Hide();

                if (disposing)
                {
                    this.UnsubscribeEvents();
                    this.DisposeComponents();

                    GameObject.DestroyImmediate(this.windowPanel.GameObject);

                    disposed = true;
                }
            }
        }

        #endregion
    }
}
