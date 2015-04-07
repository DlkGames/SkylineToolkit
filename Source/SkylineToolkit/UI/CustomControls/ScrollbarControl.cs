using ColossalFramework.UI;
using SkylineToolkit.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class ScrollbarControl : ControlsContainer
    {
        public ScrollbarControl()
            : this("Scrollbar")
        {
        }

        public ScrollbarControl(string name)
            : this(name, Vector3.zero, new Vector2(16, 100))
        {
        }

        public ScrollbarControl(string name, Vector3 position, Vector2 size)
        {
            this.Name = name;

            Initialize();

            this.Position = position;
            this.Size = size;
        }

        #region Properties

        public string Name { get; set; }

        public Scrollbar Control { get; set; }

        public UIComponent UIComponent
        {
            get
            {
                if (Control == null)
                {
                    return null;
                }

                return Control.UIComponent;
            }
        }

        public SlicedSprite Track { get; set; }

        public SlicedSprite Thumb { get; set; }

        public Orientation Orientation
        {
            get {
                return Control.Orientation;
            }
            set {
                Control.Size = new Vector2(Control.Height, Control.Width);

                Control.Orientation = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return Control.MaxValue;
            }
            set
            {
                Control.MaxValue = value;
            }
        }

        public float MinValue
        {
            get
            {
                return Control.MinValue;
            }
            set
            {
                Control.MinValue = value;
            }
        }

        public float Value
        {
            get
            {
                return Control.Value;
            }
            set
            {
                Control.Value = value;
            }
        }

        public float StepSize
        {
            get
            {
                return Control.StepSize;
            }
            set
            {
                Control.StepSize = value;
            }
        }

        public float IncrementAmount
        {
            get
            {
                return Control.IncrementAmount;
            }
            set
            {
                Control.IncrementAmount = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return Control.RelativePosition;
            }
            set
            {
                Control.RelativePosition = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return Control.Size;
            }
            set
            {
                Control.Size = value;
            }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            Control = new Scrollbar(this.Name);

            Track = new SlicedSprite("Track");
            Control.AttachControl(Track);
            Track.SpriteName = ColossalSprite.ScrollbarTrack;
            Track.Color = new Color32(254, 254, 254, 255);
            Track.DisabledColor = new Color32(254, 254, 254, 255);
            Track.FillAmount = 1;
            Track.FillDirection = FillDirection.Horizontal;
            Track.Anchor = PositionAnchor.All;
            Track.Size = Control.Size;
            Control.TrackControl = Track;

            Thumb = new SlicedSprite("Thumb");
            Control.AttachControl(Thumb);
            Thumb.SpriteName = ColossalSprite.ScrollbarThumb;
            Thumb.Color = new Color32(254, 254, 254, 255);
            Thumb.DisabledColor = new Color32(254, 254, 254, 255);
            Thumb.FillAmount = 1;
            Thumb.FillDirection = FillDirection.Horizontal;
            Thumb.Anchor = PositionAnchor.Top | PositionAnchor.Left | PositionAnchor.Right;
            Thumb.Width = Track.Width - 6;
            Thumb.Position = new Vector3(3, 0);
            Control.ThumbControl = Thumb;
        }

        public override void AttachTo(GameObject gameObject)
        {
            Control.GameObject.transform.parent = gameObject.transform.parent;
        }

        public override void AttachTo(IColossalControl control)
        {
            control.AttachControl(Control);
        }

        #endregion

        public override void Dispose()
        {
            Control.Dispose();
            Track.Dispose();
            Thumb.Dispose();
        }
    }
}
