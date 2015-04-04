using ColossalFramework.UI;
using System;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class ColossalUserControl : ColossalTextControl
    {
        public ColossalUserControl(UIInteractiveComponent component)
            : base(component)
        {
        }

        public ColossalUserControl(string name )
            : base(name, typeof(UIInteractiveComponent))
        {
        }

        public ColossalUserControl(string name, Type componentType)
            : base(name, componentType)
        {
        }

        public ColossalUserControl(IColossalControl control)
            : base(control)
        {
        }

        public new UIInteractiveComponent UIComponent
        {
            get
            {
                return (UIInteractiveComponent)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        public bool IsFocusable
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

        #region Sprites

        public string DisabledBackgroundSprite
        {
            get
            {
                return this.UIComponent.disabledBgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.disabledBgSprite = value;
            }
        }

        public string DisabledForegroundSprite
        {
            get
            {
                return this.UIComponent.disabledFgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.disabledFgSprite = value;
            }
        }

        public string FocusedBackgroundSprite
        {
            get
            {
                return this.UIComponent.focusedBgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.focusedBgSprite = value;
            }
        }

        public string FocusedForegroundSprite
        {
            get
            {
                return this.UIComponent.focusedFgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.focusedFgSprite = value;
            }
        }

        public string HoveredBackgroundSprite
        {
            get
            {
                return this.UIComponent.hoveredBgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.hoveredBgSprite = value;
            }
        }

        public string HoveredForegroundSprite
        {
            get
            {
                return this.UIComponent.hoveredFgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.hoveredFgSprite = value;
            }
        }

        public string NormalBackgroundSprite
        {
            get
            {
                return this.UIComponent.normalBgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.normalBgSprite = value;
            }
        }

        public string NormalForegroundSprite
        {
            get
            {
                return this.UIComponent.normalFgSprite;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.UIComponent.normalFgSprite = value;
            }
        }

        #endregion

        public float ScaleFactor
        {
            get
            {
                return this.UIComponent.scaleFactor;
            }
            set
            {
                this.UIComponent.scaleFactor = value;
            }
        }

        public Vector2 CalculatedMinimumSize
        {
            get
            {
                return this.UIComponent.CalculateMinimumSize();
            }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return (UI.HorizontalAlignment)this.UIComponent.horizontalAlignment;
            }
            set
            {
                this.UIComponent.horizontalAlignment = (UIHorizontalAlignment)value;
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return (UI.VerticalAlignment)this.UIComponent.verticalAlignment;
            }
            set
            {
                this.UIComponent.verticalAlignment = (UIVerticalAlignment)value;
            }
        }
    }
}
