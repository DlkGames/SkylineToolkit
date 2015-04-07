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
    public class TabStrip : ColossalControl
    {
        #region Events 

        public event PropChangedEventHandler<int> SelectedIndexChanged;

        #endregion

        #region Constructors

        public TabStrip()
            : base("TabStrip", typeof(UITabstrip))
        {
        }

        public TabStrip(string name)
            : this(name, new Vector3(0f,0f))
        {
        }

        public TabStrip(string name, Vector3 position)
            : this(name, position, new Vector2(600, 27))
        {
        }

        public TabStrip(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UITabstrip))
        {
            this.Position = position;
            this.Size = size;
        }

        public TabStrip(UITabstrip tabStrip, bool subschribeEvents = false)
            : base(tabStrip, subschribeEvents)
        {
        }

        public TabStrip(IColossalControl control, bool subschribeEvents = false) 
            : base(control, subschribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UITabstrip UIComponent
        {
            get
            {
                return (UITabstrip)base.UIComponent;
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

        public ColossalControl CloseButton
        {
            get
            {
                return this.UIComponent.closeButton.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.closeButton = value.UIComponent;
            }
        }

        public bool CloseOnSecondClick
        {
            get
            {
                return this.UIComponent.closeOnReclick;
            }
            set
            {
                this.UIComponent.closeOnReclick = value;
            }
        }

        public bool NavigateWithKeys
        {
            get
            {
                return this.UIComponent.navigateWithArrowTabKeys;
            }
            set
            {
                this.UIComponent.navigateWithArrowTabKeys = value;
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

        public int SelectedIndex
        {
            get
            {
                return this.UIComponent.selectedIndex;
            }
            set
            {
                this.UIComponent.selectedIndex = value;
            }
        }

        public Button SelectedItem
        {
            get
            {
                return new Button(this.UIComponent.tabs.OfType<UIButton>().ElementAt(this.SelectedIndex));
            }
            set
            {
                this.SelectedIndex = this.UIComponent.tabs.ToList().IndexOf(value.UIComponent);
            }
        }

        public int InitiallySelectedIndex
        {
            get
            {
                return this.UIComponent.startSelectedIndex;
            }
            set
            {
                this.UIComponent.startSelectedIndex = value;
            }
        }

        public TabContainer Container
        {
            get
            {
                return new TabContainer(this.UIComponent.tabPages);
            }
            set
            {
                this.UIComponent.tabPages = value.UIComponent;
            }
        }

        public int TabCount
        {
            get
            {
                return this.UIComponent.tabCount;
            }
        }

        public ColossalControl[] Tabs
        {
            get
            {
                return this.UIComponent.tabs.Select(t => t.ToSkylineToolkitControl()).ToArray();
            }
        }

        #endregion

        #region Methods

        public virtual Button AddTab()
        {
            Button result = new Button(this.UIComponent.AddTab());

            ApplyDefaultTabStyles(result);

            return result;
        }

        public virtual Button AddTab(string label)
        {
            Button result = new Button(this.UIComponent.AddTab(label));

            ApplyDefaultTabStyles(result);

            return result;
        }

        public virtual ColossalControl AddTab(string name, GameObject strip, GameObject page, params Type[] customControls)
        {
            return this.UIComponent.AddTab(name, strip, page, customControls).ToSkylineToolkitControl();
        }

        public virtual Button AddTab(string label, bool fillText)
        {
            Button result = new Button(this.UIComponent.AddTab(label, fillText));

            ApplyDefaultTabStyles(result);

            return result;
        }

        public virtual Button AddTab(string label, Button template, bool fillText)
        {
            Button result = new Button(this.UIComponent.AddTab(label, template.UIComponent, fillText));

            ApplyDefaultTabStyles(result);

            return result;
        }

        protected void ApplyDefaultTabStyles(Button result)
        {
            result.Anchor = Anchor.Top | Anchor.Left;
            result.BottomColor = new Color32(73, 73, 73, 255);
            result.NormalBackgroundSprite = ColossalSprite.GenericTab;
            result.Color = new Color32(255, 255, 255, 255);
            result.TextColor = new Color32(255, 255, 255, 255);
            result.DisabledBackgroundSprite = ColossalSprite.GenericTabDisabled;
            result.DisabledBottomColor = new Color32(255, 255, 255, 255);
            result.DisabledColor = new Color32(255, 255, 255, 255);
            result.DisabledTextColor = new Color32(255, 255, 255, 255);
            result.FocusedBackgroundSprite = ColossalSprite.GenericTabFocused;
            result.FocusedColor = new Color32(255, 255, 255, 255);
            result.FocusedTextColor = new Color32(0, 0, 0, 255);
            result.HoveredBackgroundSprite = ColossalSprite.GenericTabHovered;
            result.HoveredColor = new Color32(255, 255, 255, 255);
            result.HoveredTextColor = new Color32(255, 255, 255, 255);
            result.PressedBackgroundSprite = ColossalSprite.GenericTabPressed;
            result.PressedColor = new Color32(255, 255, 255, 255);
            result.PressedTextColor = new Color32(255, 255, 255, 255);
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.OutlineColor = new Color32(0, 0, 0, 255);
            result.OutlineSize = 4;
            result.VerticalAlignment = VerticalAlignment.Middle;
            result.ZOrder = 1;

            result.Height = 26;
        }

        protected virtual void ArrangeTabs()
        {
            CallUIComponentMethod<object>("ArrangeTabs");
        }

        public virtual void EnableTab(int index)
        {
            this.UIComponent.EnableTab(index);
        }

        public virtual void DisableTab(int index)
        {
            this.UIComponent.DisableTab(index);
        }

        public object GetComponentInContainer(UIComponent controller, Type type)
        {
            return this.UIComponent.GetComponentInContainer(controller, type);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.UIComponent.eventSelectedIndexChanged += OnSelectedIndexChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            this.UIComponent.eventSelectedIndexChanged -= OnSelectedIndexChanged;
        }

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.Anchor = Anchor.Top | Anchor.Left | Anchor.Right;

        }

        #endregion

        #region Event wrappers

        protected void OnSelectedIndexChanged(UIComponent component, int e)
        {
            if (this.SelectedIndexChanged != null)
            {
                PropChangedEventArgs<int> args = new PropChangedEventArgs<int>("SelectedIndex", e);

                this.SelectedIndexChanged(this, args);
            }
        }

        #endregion
    }
}
