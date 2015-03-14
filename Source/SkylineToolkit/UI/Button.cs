using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Button : ColossalControl<UIButton>
    {
        public Button(string label)
            : base("btn_" + label)
        {
            this.UIComponent.text = label;
        }

        public Button(string label, float x, float y, float width = 120, float height = 30)
            : this(label)
        {
            this.UIComponent.transformPosition = new Vector3(x, y);

            this.UIComponent.width = width;
            this.UIComponent.height = height;
        }

        public bool AutoSize
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
                return new ColossalControl<UIComponent>(this.UIComponent.group);
            }
            set
            {
                this.UIComponent.group = value.UIComponent;
            }
        }

        public void Invalidate()
        {
            this.UIComponent.Invalidate();
        }
    }
}
