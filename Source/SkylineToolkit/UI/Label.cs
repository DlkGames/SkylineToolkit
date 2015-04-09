using ColossalFramework.UI;
using SkylineToolkit.Events;
using System.Collections.Generic;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Label : ColossalTextControl
    {
        #region Events 

        public event PropChangedEventHandler<string> TextChanged;

        #endregion

        #region Constructors

        public Label()
            : base("Label", typeof(UILabel))
        {
        }

        public Label(string name)
            : this(name, name, new Vector3(0f,0f))
        {
        }

        public Label(string name, string text)
            : this(name, text, new Vector3(0f, 0f))
        {
        }

        public Label(string name, string text, Vector3 position)
            : this(name, text, position, new Vector2(80, 20))
        {
        }

        public Label(string name, string label, Vector3 position, Vector2 size)
            : base(name, typeof(UILabel))
        {
            this.Position = position;
            this.Size = size;
            this.Text = label;
        }

        public Label(UILabel label, bool subscribeEvents = false)
            : base(label, subscribeEvents)
        {
        }

        public Label(IColossalControl control, bool subscribeEvents = false) 
            : base(control, subscribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UILabel UIComponent
        {
            get
            {
                return (UILabel)base.UIComponent;
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

        public bool AllowAutoHeight
        {
            get
            {
                return this.UIComponent.autoHeight;
            }
            set
            {
                this.UIComponent.autoHeight = value;
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

        public string Prefix
        {
            get
            {
                return this.UIComponent.prefix;
            }
            set
            {
                this.UIComponent.prefix = value;
            }
        }

        public string Suffix
        {
            get
            {
                return this.UIComponent.suffix;
            }
            set
            {
                this.UIComponent.suffix = value;
            }
        }

        public int TabSize
        {
            get
            {
                return this.UIComponent.tabSize;
            }
            set
            {
                this.UIComponent.tabSize = value;
            }
        }

        public IList<int> TabStops
        {
            get
            {
                return this.UIComponent.tabStops;
            }
        }

        public HorizontalAlignment HorizontalTextAlignment
        {
            get
            {
                return (HorizontalAlignment)this.UIComponent.textAlignment;
            }
            set
            {
                this.UIComponent.textAlignment = (UIHorizontalAlignment)value;
            }
        }

        public VerticalAlignment VerticalTextAlignment
        {
            get
            {
                return (VerticalAlignment)this.UIComponent.verticalAlignment;
            }
            set
            {
                this.UIComponent.verticalAlignment = (UIVerticalAlignment)value;
            }
        }

        public bool WordWrap
        {
            get
            {
                return this.UIComponent.wordWrap;
            }
            set
            {
                this.UIComponent.wordWrap = value;
            }
        }

        #endregion

        #region Methods

        protected virtual void RenderBackgorund()
        {
            this.CallUIComponentMethod<object>("RenderBackground");
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.UIComponent.eventTextChanged += OnTextChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            this.UIComponent.eventTextChanged -= OnTextChanged;
        }

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.Anchor = Anchor.CenterHorizontal | Anchor.CenterVertical;
            this.EnableAutoSize = true;
            this.BottomColor = new Color32(255, 255, 255, 255);
            this.Color = new Color32(255, 255, 255, 255);
            this.TextColor = new Color32(255, 255, 255, 255);
            this.DisabledColor = new Color32(255, 255, 255, 255);
            this.DisabledTextColor = new Color32(255, 255, 255, 255);
            this.OutlineColor = new Color32(0, 0, 0, 255);
            this.OutlineSize = 1;
            this.TabSize = 48;
            this.HorizontalTextAlignment = HorizontalAlignment.Left;
            this.TextScale = 1.125f;
            this.VerticalTextAlignment = VerticalAlignment.Top;
        }

        #endregion

        #region Event wrappers 

        protected void OnTextChanged(UIComponent component, string e)
        {
            if (this.TextChanged != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("Text", e);

                this.TextChanged(this, args);
            }
        }

        #endregion
    }
}
