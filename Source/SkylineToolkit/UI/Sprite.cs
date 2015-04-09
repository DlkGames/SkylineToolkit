using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Sprite : ColossalControl
    {
        #region Events

        public event PropChangedEventHandler<string> SpriteNameChanged;

        #endregion

        #region Constructors

        public Sprite()
            : base("Sprite", typeof(UISprite))
        {
        }

        public Sprite(string name)
            : this(name, new Vector3(0f, 0f), new Vector2(400, 200))
        {
        }

        public Sprite(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UISprite))
        {
            this.Position = position;
            this.Size = size;
        }

        public Sprite(UISprite sprite, bool subschribeEvents = false)
            : base(sprite, subschribeEvents)
        {
        }

        public Sprite(IColossalControl control, bool subschribeEvents = false)
            : base(control, subschribeEvents)
        {
        }

        protected internal Sprite(string name, Type componentType)
            : base(name, componentType)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UISprite UIComponent
        {
            get
            {
                return (UISprite)base.UIComponent;
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

        public float FillAmount
        {
            get
            {
                return this.UIComponent.fillAmount;
            }
            set
            {
                this.UIComponent.fillAmount = value;
            }
        }

        public FillDirection FillDirection
        {
            get
            {
                return (FillDirection)this.UIComponent.fillDirection;
            }
            set
            {
                this.UIComponent.fillDirection = (UIFillDirection)value;
            }
        }

        public SpriteFlipping Flip
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

        public bool InvertFill
        {
            get
            {
                return this.UIComponent.invertFill;
            }
            set
            {
                this.UIComponent.invertFill = value;
            }
        }
        
        // TODO Create proper wrapper for UITextureAtlas
        public UITextureAtlas.SpriteInfo SpriteInfo
        {
            get
            {
                return this.UIComponent.spriteInfo;
            }
        }

        public string SpriteName
        {
            get
            {
                return this.UIComponent.spriteName;
            }
            set
            {
                this.UIComponent.spriteName = value;
            }
        }

        #endregion

        #region Methods

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.UIComponent.eventSpriteNameChanged += OnSpriteNameChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            this.UIComponent.eventSpriteNameChanged -= OnSpriteNameChanged;
        }

        #endregion

        #region Event wrappers

        protected void OnSpriteNameChanged(UIComponent component, string e)
        {
            if (this.SpriteNameChanged != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("SpriteName", e);

                this.SpriteNameChanged(this, args);
            }
        }

        #endregion
    }
}
