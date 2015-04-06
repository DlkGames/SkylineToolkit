using ColossalFramework.UI;
using System;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class ColossalTextControl : ColossalControl
    {
        public ColossalTextControl(string name)
            : base(name, typeof(UITextComponent))
        {
        }

        public ColossalTextControl(string name, Type componentType)
            : base(name, componentType)
        {
        }

        public ColossalTextControl(IColossalControl control, bool subscribeEvents = false)
            : base(control, subscribeEvents)
        {
        }

        public ColossalTextControl(UITextComponent component, bool subscribeEvents = false)
            : base(component, subscribeEvents)
        {
        }

        public new UITextComponent UIComponent
        {
            get
            {
                return (UITextComponent)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #region Colors

        public Color32 BottomColor
        {
            get
            {
                return this.UIComponent.bottomColor;
            }
            set
            {
                this.UIComponent.bottomColor = value;
            }
        }

        public Color32 DisabledTextColor
        {
            get
            {
                return this.UIComponent.disabledTextColor;
            }
            set
            {
                this.UIComponent.disabledTextColor = value;
            }
        }

        public Color32 DropShadowColor
        {
            get
            {
                return this.UIComponent.dropShadowColor;
            }
            set
            {
                this.UIComponent.dropShadowColor = value;
            }
        }

        public Color32 OutlineColor
        {
            get
            {
                return this.UIComponent.outlineColor;
            }
            set
            {
                this.UIComponent.outlineColor = value;
            }
        }

        public Color32 TextColor
        {
            get
            {
                return this.UIComponent.textColor;
            }
            set
            {
                this.UIComponent.textColor = value;
            }
        }

        #endregion

        public int CharacterSpacing
        {
            get
            {
                return this.UIComponent.characterSpacing;
            }
            set
            {
                this.UIComponent.characterSpacing = value;
            }
        }

        public bool ColorizeSprites
        {
            get
            {
                return this.UIComponent.colorizeSprites;
            }
            set
            {
                this.UIComponent.colorizeSprites = value;
            }
        }

        public Vector2 DropShadowOffset
        {
            get
            {
                return this.UIComponent.dropShadowOffset;
            }
            set
            {
                this.UIComponent.dropShadowOffset = value;
            }
        }

        public string LocaleId
        {
            get
            {
                return this.UIComponent.localeID;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.localeID = value;
            }
        }

        public int OutlineSize
        {
            get
            {
                return this.UIComponent.outlineSize;
            }
            set
            {
                this.UIComponent.outlineSize = value;
            }
        }

        public bool ProcessMarkup
        {
            get
            {
                return this.UIComponent.processMarkup;
            }
            set
            {
                this.UIComponent.processMarkup = value;
            }
        }

        public string Text
        {
            get
            {
                return this.UIComponent.text;
            }
            set
            {
                this.UIComponent.text = value;
            }
        }

        public float TextScale
        {
            get
            {
                return this.UIComponent.textScale;
            }
            set
            {
                this.UIComponent.textScale = value;
            }
        }

        public bool UseDropShadows
        {
            get
            {
                return this.UIComponent.useDropShadow;
            }
            set
            {
                this.UIComponent.useDropShadow = value;
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.UIComponent.useGradient;
            }
            set
            {
                this.UIComponent.useGradient = value;
            }
        }

        public bool UseOutline
        {
            get
            {
                return this.UIComponent.useOutline;
            }
            set
            {
                this.UIComponent.useOutline = value;
            }
        }

        protected virtual float GetTextScaleMultiplier()
        {
            return CallUIComponentMethod<float>("GetTextScaleMultiplier");
        }

        protected virtual void RequestCharacterInfo()
        {
            CallUIComponentMethod<object>("RequestCharacterInfo");
        }

        public virtual void UpdateFontInfo()
        {
            this.UIComponent.UpdateFontInfo();
        }
    }
}
