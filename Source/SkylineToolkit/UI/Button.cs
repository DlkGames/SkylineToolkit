using ColossalFramework.UI;
using SkylineToolkit.UI.Styles;
using System;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Button : ColossalUserControl
    {
        #region Constructors

        public Button()
            : base("Button", typeof(UIButton))
        {
        }

        public Button(string name)
            : this(name, name, new Vector3(0f,0f))
        {
        }

        public Button(string name, string text)
            : this(name, text, new Vector3(0f, 0f))
        {
        }

        public Button(string name, string label, Vector3 position)
            : this(name, label, position, new Vector2(120, 34))
        {
        }

        public Button(string name, string label, Vector3 position, Vector2 size)
            : base(name, typeof(UIButton))
        {
            this.Position = position;
            this.Size = size;
            this.Text = label;
        }

        public Button(UIButton button, bool subscribeEvents = false)
            : base(button, subscribeEvents)
        {
        }

        public Button(IColossalControl control, bool subscribeEvents = false) 
            : base(control, subscribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UIButton UIComponent
        {
            get
            {
                return (UIButton)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        #region Colors

        public Color32 DisabledBottomColor
        {
            get
            {
                return this.UIComponent.disabledBottomColor;
            }
            set
            {
                this.UIComponent.disabledBottomColor = value;
            }
        }

        public Color32 FocusedColor
        {
            get
            {
                return this.UIComponent.focusedColor;
            }
            set
            {
                this.UIComponent.focusedColor = value;
            }
        }

        public Color32 FocusedTextColor
        {
            get
            {
                return this.UIComponent.focusedTextColor;
            }
            set
            {
                this.UIComponent.focusedTextColor = value;
            }
        }

        public Color32 HoveredColor
        {
            get
            {
                return this.UIComponent.hoveredColor;
            }
            set
            {
                this.UIComponent.hoveredColor = value;
            }
        }

        public Color32 HoveredTextColor
        {
            get
            {
                return this.UIComponent.hoveredTextColor;
            }
            set
            {
                this.UIComponent.hoveredTextColor = value;
            }
        }

        public Color32 PressedColor
        {
            get
            {
                return this.UIComponent.pressedColor;
            }
            set
            {
                this.UIComponent.pressedColor = value;
            }
        }

        public Color32 PressedTextColor
        {
            get
            {
                return this.UIComponent.pressedTextColor;
            }
            set
            {
                this.UIComponent.pressedTextColor = value;
            }
        }

        #endregion

        #region Sprites

        public string PressedBackgroundSprite
        {
            get
            {
                return this.UIComponent.pressedBgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.pressedBgSprite = value;
            }
        }

        public string PressedForegroundSprite
        {
            get
            {
                return this.UIComponent.pressedFgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.pressedFgSprite = value;
            }
        }

        #endregion

        #region State

        public ButtonState State
        {
            get
            {
                return (ButtonState)this.UIComponent.state;
            }
            set
            {
                this.UIComponent.state = (UIButton.ButtonState)value;
            }
        }

        public bool TabStrip
        {
            get
            {
                return this.UIComponent.tabStrip;
            }
            set
            {
                this.UIComponent.tabStrip = value;
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

        public IColossalControl Group
        {
            get
            {
                return new ColossalControl(this.UIComponent.group);
            }
            set
            {
                this.UIComponent.group = value.UIComponent;
            }
        }

        #endregion

        public HorizontalAlignment HorizontalTextAlignment
        {
            get
            {
                return (UI.HorizontalAlignment)this.UIComponent.textHorizontalAlignment;
            }
            set
            {
                this.UIComponent.textHorizontalAlignment = (UIHorizontalAlignment)value;
            }
        }

        public VerticalAlignment VerticalTextAlignment
        {
            get
            {
                return (UI.VerticalAlignment)this.UIComponent.textVerticalAlignment;
            }
            set
            {
                this.UIComponent.textVerticalAlignment = (UIVerticalAlignment)value;
            }
        }

        public RectOffset Padding
        {
            get
            {
                return this.UIComponent.textPadding;
            }
            set
            {
                this.UIComponent.textPadding = value;
            }
        }

        public RectOffset SpritePadding
        {
            get
            {
                return this.UIComponent.spritePadding;
            }
            set
            {
                this.UIComponent.spritePadding = value;
            }
        }

        #endregion

        #region Methods

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.DisabledBackgroundSprite = ColossalSprite.ButtonMenuDisabled;
            this.DisabledBottomColor = new Color32(255, 255, 255, 255);
            this.DisabledColor = new Color32(153, 153, 153, 255);
            this.DisabledTextColor = new Color32(46, 46, 46, 255);
            this.FocusedBackgroundSprite = ColossalSprite.ButtonMenuFocused; // TODO Focused style
            this.FocusedColor = new Color32(254, 254, 254, 255);
            this.FocusedTextColor = new Color32(255, 255, 255, 255);
            this.HoveredBackgroundSprite = ColossalSprite.ButtonMenuHovered;
            this.HoveredColor = new Color32(254, 254, 254, 255);
            this.HoveredTextColor = new Color32(7, 132, 255, 255);
            this.NormalBackgroundSprite = ColossalSprite.ButtonMenu;
            this.BottomColor = new Color32(254, 254, 254, 255);
            this.Color = new Color32(254, 254, 254, 255);
            this.TextColor = new Color32(255, 255, 255, 255);
            this.PressedBackgroundSprite = ColossalSprite.ButtonMenuPressed;
            this.PressedColor = new Color32(254, 254, 254, 255);
            this.PressedTextColor = new Color32(30, 30, 44, 255);
            this.IsPlayingAudioEvents = true;
            this.HorizontalTextAlignment = HorizontalAlignment.Center;
            this.TextScale = 1.0f;
            this.VerticalTextAlignment = VerticalAlignment.Middle;
            this.ZOrder = 9;
        }

        #endregion
    }
}
