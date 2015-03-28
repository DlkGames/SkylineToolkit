using ColossalFramework.UI;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Panel : ColossalControl
    {
        public Panel()
            : base("Panel", typeof(UIPanel))
        {
            SetDefaultStyle();
        }

        public Panel(string name)
            : this(name, name, new Vector3(0f, 0f), new Vector2(400, 200))
        {
        }

        public Panel(string name, string label, Vector3 position, Vector2 size)
            : base(name, typeof(UIPanel))
        {
            this.Position = position;
            this.Size = size;

            SetDefaultStyle();
        }

        //public Panel(UIPanel panel)
        //    : base(panel)
        //{
        //}

        public Panel(IColossalControl control)
            : base(control)
        {
        }

        public new UIPanel UIComponent
        {
            get
            {
                return (UIPanel)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        public bool AutoFitChildrenHorizontally
        {
            get
            {
                return this.UIComponent.autoFitChildrenHorizontally;
            }
            set
            {
                this.UIComponent.autoFitChildrenHorizontally = value;
            }
        }

        public bool AutoFitChildrenVertically
        {
            get
            {
                return this.UIComponent.autoFitChildrenVertically;
            }
            set
            {
                this.UIComponent.autoFitChildrenVertically = value;
            }
        }

        public bool AutoLayout
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
            set {
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

        public SpriteFlipping FlipBackground
        {
            get
            {
                return (SpriteFlipping)this.UIComponent.flip;
            }
            set
            {
                this.UIComponent.flip = (UISpriteFlip)value;
            }
        }

        public RectOffset Padding
        {
            get
            {
                return this.UIComponent.padding;
            }
            set
            {
                this.UIComponent.padding = value;
            }
        }

        public bool IsUsingCenter
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

        public bool IsWrappingLayout
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

        public void Reset()
        {
            this.UIComponent.Reset();
        }

        public void SetDefaultStyle()
        {
            this.BackgroundSprite = "MenuPanel";
        }
    }
}
