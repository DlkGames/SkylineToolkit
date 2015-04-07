using ColossalFramework.UI;
using SkylineToolkit.Events;
using SkylineToolkit.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class ScrollablePanel : ColossalControl
    {
        #region Events

        public event PropChangedEventHandler<Vector2> ScrollPositionChanged;

        #endregion

        #region Constructors

        public ScrollablePanel()
            : base("ScrollablePanel", typeof(UIScrollablePanel))
        {
        }

        public ScrollablePanel(string name)
            : this(name, new Vector3(0f, 0f), new Vector2(400, 200))
        {
        }

        public ScrollablePanel(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UIScrollablePanel))
        {
            this.Position = position;
            this.Size = size;
        }

        public ScrollablePanel(UIScrollablePanel panel, bool subschribeEvents = false)
            : base(panel, subschribeEvents)
        {
        }

        public ScrollablePanel(IColossalControl control, bool subschribeEvents = false)
            : base(control, subschribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UIScrollablePanel UIComponent
        {
            get
            {
                return (UIScrollablePanel)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        // TODO Create proper wrapper for UITextureAtlas
        public UITextureAtlas TextureAtlas
        {
            get
            {
                return this.UIComponent.atlas;
            }
            set
            {
                this.UIComponent.atlas = value;
            }
        }

        public bool EnableAutoLayout
        {
            get
            {
                return this.UIComponent.autoLayout;
            }
            set
            {
                this.UIComponent.autoLayout = value;
            }
        }

        public LayoutDirection AutoLayoutDirection
        {
            get
            {
                return (LayoutDirection)this.UIComponent.autoLayoutDirection;
            }
            set
            {
                this.UIComponent.autoLayoutDirection = (ColossalFramework.UI.LayoutDirection)value;
            }
        }

        public RectOffset AutoLayoutPadding
        {
            get
            {
                return this.UIComponent.autoLayoutPadding;
            }
            set
            {
                this.UIComponent.autoLayoutPadding = value;
            }
        }

        public LayoutOrigin AutoLayoutOrigin
        {
            get
            {
                return (LayoutOrigin)this.UIComponent.autoLayoutStart;
            }
            set
            {
                this.UIComponent.autoLayoutStart = (LayoutStart)value;
            }
        }

        public bool EnableAutoReset
        {
            get
            {
                return this.UIComponent.autoReset;
            }
            set
            {
                this.UIComponent.autoReset = value;
            }
        }

        public string BackgroundSprite
        {
            get
            {
                return this.UIComponent.backgroundSprite;
            }
            set
            {
                this.UIComponent.backgroundSprite = value;
            }
        }

        public bool EnableFreeScroll
        {
            get
            {
                return this.UIComponent.freeScroll;
            }
            set
            {
                this.UIComponent.freeScroll = value;
            }
        }

        public Scrollbar HorizontalScrollbar
        {
            get
            {
                return new Scrollbar(this.UIComponent.horizontalScrollbar);
            }
            set
            {
                this.UIComponent.horizontalScrollbar = value.UIComponent;
            }
        }

        public Scrollbar VerticalScrollbar
        {
            get
            {
                return new Scrollbar(this.UIComponent.verticalScrollbar);
            }
            set
            {
                this.UIComponent.verticalScrollbar = value.UIComponent;
            }
        }

        public RectOffset ScrollPadding
        {
            get
            {
                return this.UIComponent.scrollPadding;
            }
            set
            {
                this.UIComponent.scrollPadding = value;
            }
        }

        public Vector2 ScrollPosition
        {
            get
            {
                return this.UIComponent.scrollPosition;
            }
            set
            {
                this.UIComponent.scrollPosition = value;
            }
        }

        public int ScrollWheelAmount
        {
            get
            {
                return this.UIComponent.scrollWheelAmount;
            }
            set
            {
                this.UIComponent.scrollWheelAmount = value;
            }
        }

        public Orientation ScrollWheelDirection
        {
            get
            {
                return (Orientation)this.UIComponent.scrollWheelDirection;
            }
            set
            {
                this.UIComponent.scrollWheelDirection = (UIOrientation)value;
            }
        }

        public bool EnableArrowKeyScrolling
        {
            get
            {
                return this.UIComponent.scrollWithArrowKeys;
            }
            set
            {
                this.UIComponent.scrollWithArrowKeys = value;
            }
        }

        public bool UseCenter
        {
            get
            {
                return this.UIComponent.useCenter;
            }
            set
            {
                this.UIComponent.useCenter = value;
            }
        }

        public bool UseScrollMomentum
        {
            get
            {
                return this.UIComponent.useScrollMomentum;
            }
            set
            {
                this.UIComponent.useScrollMomentum = value;
            }
        }

        public bool UseTouchMouseScroll
        {
            get
            {
                return this.UIComponent.useTouchMouseScroll;
            }
            set
            {
                this.UIComponent.useTouchMouseScroll = value;
            }
        }

        public bool EnableLayoutWrapping
        {
            get
            {
                return this.UIComponent.wrapLayout;
            }
            set
            {
                this.UIComponent.wrapLayout = value;
            }
        }

        public Vector2 ViewSize
        {
            get
            {
                return this.CalculateViewSize();
            }
        }

        #endregion

        #region Methods

        public virtual Vector2 CalculateViewSize()
        {
            return this.UIComponent.CalculateViewSize();
        }

        public void CenterChildControls()
        {
            this.UIComponent.CenterChildControls();
        }

        public void FitToContents()
        {
            this.UIComponent.FitToContents();
        }

        public void Reset()
        {
            this.UIComponent.Reset();
        }

        public void ScrollToControl(UIComponent component)
        {
            this.UIComponent.ScrollIntoView(component);
        }

        public void ScrollToControl(IColossalControl control)
        {
            this.ScrollToControl(control.UIComponent);
        }

        public void ScrollToTop()
        {
            this.UIComponent.ScrollToTop();
        }

        public void ScrollToBottom()
        {
            this.UIComponent.ScrollToBottom();
        }

        public void ScrollToLeft()
        {
            this.UIComponent.ScrollToLeft();
        }

        public void ScrollToRight()
        {
            this.UIComponent.ScrollToRight();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.UIComponent.eventScrollPositionChanged += OnScrollPositionChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            this.UIComponent.eventScrollPositionChanged -= OnScrollPositionChanged;
        }

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.IsClippingChildren = true;
            this.EnableAutoLayout = false;
            this.AutoLayoutDirection = LayoutDirection.Horizontal;
            this.AutoLayoutOrigin = LayoutOrigin.TopLeft;
            this.BackgroundSprite = ColossalSprite.GenericPanel;
            this.Color = new Color32(7, 7, 7, 255);
            this.ScrollWheelAmount = 50;
            this.ScrollWheelDirection = Orientation.Vertical;
        }

        #endregion

        #region Event wrappers

        protected void OnScrollPositionChanged(UIComponent component, Vector2 e)
        {
            if (this.ScrollPositionChanged != null)
            {
                PropChangedEventArgs<Vector2> args = new PropChangedEventArgs<Vector2>("ScrollPosition", e);

                this.ScrollPositionChanged(this, args);
            }
        }

        #endregion
    }
}
