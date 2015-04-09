using ColossalFramework.UI;
using SkylineToolkit.Events;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Scrollbar : ColossalControl
    {
        #region Events

        public event PropChangedEventHandler<float> ValueChanged;

        #endregion
        
        #region Constructors

        public Scrollbar()
            : base("Scrollbar", typeof(UIScrollbar))
        {
        }

        public Scrollbar(string name)
            : this(name, new Vector3(0f, 0f), new Vector2(16, 100))
        {
        }

        public Scrollbar(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UIScrollbar))
        {
            this.Position = position;
            this.Size = size;
        }

        public Scrollbar(UIScrollbar scrollbar, bool subschribeEvents = false)
            : base(scrollbar, subschribeEvents)
        {
        }

        public Scrollbar(IColossalControl control, bool subschribeEvents = false)
            : base(control, subschribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UIScrollbar UIComponent
        {
            get
            {
                return (UIScrollbar)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        public bool EnableAutoHide
        {
            get
            {
                return this.UIComponent.autoHide;
            }
            set
            {
                this.UIComponent.autoHide = value;
            }
        }

        public IColossalControl IncrementButton
        {
            get
            {
                return this.UIComponent.incrementButton.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.incrementButton = value.UIComponent;
            }
        }

        public IColossalControl DecrementButton
        {
            get
            {
                return this.UIComponent.decrementButton.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.decrementButton = value.UIComponent;
            }
        }

        public float IncrementAmount
        {
            get
            {
                return this.UIComponent.incrementAmount;
            }
            set
            {
                this.UIComponent.incrementAmount = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return this.UIComponent.maxValue;
            }
            set
            {
                this.UIComponent.maxValue = value;
            }
        }

        public float MinValue
        {
            get
            {
                return this.UIComponent.minValue;
            }
            set
            {
                this.UIComponent.minValue = value;
            }
        }

        // TODO.. maybe the same as LayoutDirection?
        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.UIComponent.orientation;
            }
            set
            {
                this.UIComponent.orientation = (UIOrientation)value;
            }
        }

        public float EasingTime
        {
            get
            {
                return this.UIComponent.scrollEasingTime;
            }
            set
            {
                this.UIComponent.scrollEasingTime = value;
            }
        }

        public EasingType EasingType
        {
            get
            {
                return (EasingType)this.UIComponent.scrollEasingType;
            }
            set
            {
                this.UIComponent.scrollEasingType = (ColossalFramework.EasingType)value;
            }
        }

        public float ScrollSize
        {
            get
            {
                return this.UIComponent.scrollSize;
            }
            set
            {
                this.UIComponent.scrollSize = value;
            }
        }

        public float StepSize
        {
            get
            {
                return this.UIComponent.stepSize;
            }
            set
            {
                this.UIComponent.stepSize = value;
            }
        }

        public IColossalControl ThumbControl
        {
            get
            {
                return this.UIComponent.thumbObject.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.thumbObject = value.UIComponent;
            }
        }

        public IColossalControl TrackControl
        {
            get
            {
                return this.UIComponent.trackObject.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.trackObject = value.UIComponent;
            }
        }

        public RectOffset ThumbPadding
        {
            get
            {
                return this.UIComponent.thumbPadding;
            }
            set
            {
                this.UIComponent.thumbPadding = value;
            }
        }

        public float Value
        {
            get
            {
                return this.UIComponent.value;
            }
            set
            {
                this.UIComponent.value = value;
            }
        }

        #endregion

        #region Methods

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.UIComponent.eventValueChanged += OnValueChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            this.UIComponent.eventValueChanged -= OnValueChanged;
        }

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.EnableAutoHide = false;
            this.EnableAutoSize = false;
            this.Color = new Color32(255, 255, 255, 255);
            this.DisabledColor = new Color32(255, 255, 255, 255);
            this.IncrementAmount = 50;
            this.Orientation = UI.Orientation.Vertical;
            this.EasingTime = 1;
            this.EasingType = UI.EasingType.None;
        }

        #endregion

        #region Event wrappers

        protected void OnValueChanged(UIComponent component, float e)
        {
            if (this.ValueChanged != null)
            {
                PropChangedEventArgs<float> args = new PropChangedEventArgs<float>("Value", e);

                this.ValueChanged(this, args);
            }
        }

        #endregion
    }
}
