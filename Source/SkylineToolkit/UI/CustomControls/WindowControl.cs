using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using SkylineToolkit.Events;
using SkylineToolkit.UI.ActionContorls;
using SkylineToolkit.UI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class WindowControl : CustomControl, IDisposable
    {
        private Panel windowPanel;

        private DragHandle dragHandle;

        private ColossalControl<UISlicedSprite> captionComponent;

        private ColossalControl<UILabel> labelComponent;

        private Button closeButton;

        public event EventHandler<CancellableEventArgs> Close;

        public event EventHandler Closed;

        public event EventHandler<CancellableEventArgs> Open;

        public event EventHandler Opened;

        private WindowControl()
        {
        }

        public Panel WindowPanel
        {
            get { return windowPanel; }
            set { windowPanel = value; }
        }

        public bool IsVisible { get;  protected set; }

        #region Properties

        #region Controls

        public DragHandle DragHandle
        {
            get { return dragHandle; }
            set { dragHandle = value; }
        }

        public ColossalControl<UILabel> LabelComponent
        {
            get { return labelComponent; }
            set { labelComponent = value; }
        }

        public ColossalControl<UISlicedSprite> CaptionComponent
        {
            get { return captionComponent; }
            set { captionComponent = value; }
        }

        public Button CloseButton
        {
            get { return closeButton; }
            set { closeButton = value; }
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
            get
            {
                return this.windowPanel.Position;
            }
            set
            {
                this.windowPanel.Position = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return this.windowPanel.Size;
            }
            set
            {
                this.windowPanel.Size = value;
            }
        }

        #endregion

        void OnEnable()
        {
            this.InitializeWindow();
        }

        void OnDisable()
        {
            this.Hide();
        }

        public virtual void Hide()
        {
            CancellableEventArgs args = new CancellableEventArgs();

            this.OnClose(args);

            if (args.Cancel)
            {
                return;
            }

            this.windowPanel.Hide();
            this.IsVisible = false;

            this.OnClosed();
        }

        public virtual void Show()
        {
            CancellableEventArgs args = new CancellableEventArgs();

            this.OnOpen(args);

            if (args.Cancel)
            {
                return;
            }

            this.windowPanel.Show();
            this.IsVisible = true;

            this.OnOpened();
        }

        protected virtual void InitializeWindow()
        {
            windowPanel = new Panel(this.gameObject.AddComponent<UIPanel>());
            windowPanel.Anchor = PositionAnchor.None;
            windowPanel.IsInteractive = true;
            windowPanel.CanGetFocus = true;
            windowPanel.Pivot = PivotPoint.TopLeft;
            windowPanel.BackgroundSprite = "MenuPanel";

            CreateTitlebar();

            windowPanel.IsActive = true;
        }

        private void CreateTitlebar()
        {
            captionComponent = new ColossalControl<UISlicedSprite>("Caption");
            windowPanel.AttachControl(captionComponent);
            captionComponent.Width = windowPanel.Width;
            captionComponent.Height = 40;
            captionComponent.Anchor = PositionAnchor.Top | PositionAnchor.Left | PositionAnchor.Right;
            captionComponent.ZOrder = 0;
            captionComponent.IsActive = true;
            captionComponent.RelativePosition = Vector3.zero;

            dragHandle = new DragHandle("Drag Handle");
            captionComponent.AttachControl(DragHandle);
            dragHandle.Target = windowPanel;
            dragHandle.Width = windowPanel.Width;
            dragHandle.Height = 40;
            dragHandle.Anchor = PositionAnchor.All;
            dragHandle.ZOrder = 1;
            dragHandle.IsActive = true;
            dragHandle.RelativePosition = Vector3.zero;

            labelComponent = new ColossalControl<UILabel>("Label");
            captionComponent.AttachControl(labelComponent);
            labelComponent.Height = 40;
            labelComponent.Anchor = PositionAnchor.CenterHorizontal | PositionAnchor.CenterVertical;
            labelComponent.IsAutoSize = true;
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
            closeButton.OutlineColor = new Color32(0, 0, 0, 255);
            closeButton.Pivot = PivotPoint.TopLeft;
            closeButton.Anchor = PositionAnchor.Top | PositionAnchor.Right;
            closeButton.ZOrder = 2;
            closeButton.IsActive = true;
            closeButton.RelativePosition = new Vector3(this.windowPanel.Width - 36, 4);
            closeButton.Click += closeButton_Click;
        }


        private void closeButton_Click(object sender, MouseEventArgs e)
        {
            this.Hide();
        }

        #region On events

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

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Hide();

            GameObject.DestroyImmediate(this.windowPanel.GameObject);
        }

        #endregion
    }
}
