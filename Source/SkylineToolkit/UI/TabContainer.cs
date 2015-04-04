using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class TabContainer : ColossalControl
    {
        #region Events

        public event ItemAddedEventHandler<Panel> TabPageAdded;

        public event PropChangedEventHandler<int> SelectedIndexChanged;

        #endregion

        #region Constructors

        public TabContainer()
            : base("TabContainer", typeof(UITabContainer))
        {
        }

        public TabContainer(string name)
            : this(name, new Vector3(0f,0f))
        {
        }

        public TabContainer(string name, Vector3 position)
            : this(name, position, new Vector2(600, 400))
        {
        }

        public TabContainer(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UITabContainer))
        {
            this.Position = position;
            this.Size = size;
        }

        public TabContainer(UITabContainer tabContainer, bool subschribeEvents = false)
            : base(tabContainer, subschribeEvents)
        {
        }

        public TabContainer(IColossalControl control, bool subschribeEvents = false) 
            : base(control, subschribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UITabContainer UIComponent
        {
            get
            {
                return (UITabContainer)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        /// <summary>
        /// TODO Wrapper for UITextureAtlas
        /// </summary>
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

        public TabStrip Tabstrip
        {
            get
            {
                return new TabStrip(this.UIComponent.owner.ToSkylineToolkitControl());
            }
            protected internal set
            {
                SetUIComponentProperty<UITabstrip>("owner", value.UIComponent);
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

        public Panel SelectedItem
        {
            get
            {
                return new Panel(this.Children.OfType<UIPanel>().ElementAt(this.SelectedIndex));
            }
            set
            {
                this.UIComponent.selectedIndex = this.Children.ToList().IndexOf(value);
            }
        }

        #endregion

        #region Methods

        public virtual Panel AddTabPage()
        {
            Panel result = new Panel(this.UIComponent.AddTabPage().ToSkylineToolkitControl());

            this.OnTabPageAdded(result);

            return result;
        }

        public virtual Panel AddTabPage(string label)
        {
            Panel result = new Panel(this.UIComponent.AddTabPage(label).ToSkylineToolkitControl());

            this.OnTabPageAdded(result);

            return result;
        }

        public virtual Panel AddTabPage(string label, Panel template)
        {
            Panel result = new Panel(this.UIComponent.AddTabPage(label, template.UIComponent).ToSkylineToolkitControl());

            this.OnTabPageAdded(result);

            return result;
        }

        public virtual Panel AddTabPage(string name, GameObject page, params Type[] customControls)
        {
            if (page == null)
            {
                return null;
            }

            Panel result = new Panel(this.UIComponent.AddTabPage(name, page, customControls).ToSkylineToolkitControl());

            this.OnTabPageAdded(result);

            return result;
        }

        protected virtual void ArrangeTabs()
        {
            CallUIComponentMethod<object>("ArrangeTabs");
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

        #region On events

        protected virtual void OnTabPageAdded(Panel addedPanel)
        {
            if (this.TabPageAdded != null)
            {
                ItemAddedEventArgs<Panel> args = new ItemAddedEventArgs<Panel>(addedPanel);

                this.TabPageAdded(this, args);
            }
        }

        #endregion
    }
}
