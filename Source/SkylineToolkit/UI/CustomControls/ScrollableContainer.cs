using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class ScrollableContainer : ControlsContainer
    {
        private bool allowHorizontalScrolling = true;
        private bool allowVerticalScrolling = true;

        private Vector2 size;

        private float scrollbarWidth = 16;

        public ScrollableContainer()
            : this("ScrollableContainer")
        {
        }

        public ScrollableContainer(string name)
            : this(name, Vector3.zero, new Vector2(400, 200))
        {
        }

        public ScrollableContainer(string name, Vector3 position, Vector2 size)
        {
            this.Name = name;
            this.size = size;

            Initialize();

            this.Position = position;
        }

        public string Name { get; set; }

        public float ScrollbarWidth
        {
            get { return scrollbarWidth; }
            set { scrollbarWidth = value; }
        }

        public ScrollablePanel Panel { get;  protected set; }

        public ScrollbarControl HorizontalScrollbar { get; protected set; }

        public ScrollbarControl VerticalScrollbar { get; protected set; }

        public Vector3 Position
        {
            get
            {
                return Panel.Position;
            }
            set
            {
                Panel.Position = value;

                UpdatePanel();
            }
        }

        public Vector3 AbsolutePosition
        {
            get
            {
                return Panel.AbsolutePosition;
            }
            set
            {
                Panel.AbsolutePosition = value;

                UpdatePanel();
            }
        }

        public Vector3 RelativePosition
        {
            get
            {
                return Panel.RelativePosition;
            }
            set
            {
                Panel.RelativePosition = value;

                UpdatePanel();
            }
        }

        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;

                UpdatePanel();
            }
        }

        public bool AllowHorizontalScrolling
        {
            get
            {
                return allowHorizontalScrolling;
            }
            set
            {
                if (value)
                {
                    if (HorizontalScrollbar == null)
                    {
                        AddHorizontalScrollbar();

                        UpdatePanel();
                    }

                    allowHorizontalScrolling = true;
                }
                else
                {
                    if (HorizontalScrollbar != null)
                    {
                        RemoveHorizontalScrollbar();

                        UpdatePanel();
                    }

                    allowHorizontalScrolling = false;
                }
            }
        }

        public bool AllowVerticalScrolling
        {
            get
            {
                return allowVerticalScrolling;
            }
            set
            {
                if (value)
                {
                    if (VerticalScrollbar == null)
                    {
                        AddVerticalScrollbar();

                        UpdatePanel();
                    }

                    allowVerticalScrolling = true;
                }
                else
                {
                    if (VerticalScrollbar != null)
                    {
                        RemoveVerticalScrollbar();

                        UpdatePanel();
                    }

                    allowVerticalScrolling = false;
                }
            }
        }

        private void Initialize()
        {
            Panel = new ScrollablePanel(this.Name);
            Panel.Anchor = Anchor.All;

            if (allowHorizontalScrolling)
            {
                AddHorizontalScrollbar();
            }

            if (allowVerticalScrolling)
            {
                AddVerticalScrollbar();
            }

            UpdatePanel();
        }

        private void UpdatePanel()
        {
            if (allowHorizontalScrolling && HorizontalScrollbar == null)
            {
                allowHorizontalScrolling = false;
            }

            if (allowVerticalScrolling && VerticalScrollbar == null)
            {
                allowVerticalScrolling = false;
            }

            float panelWidth = this.size.x - (allowVerticalScrolling ? VerticalScrollbar.Width : 0);
            float panelHeight = this.size.y - (allowHorizontalScrolling ? HorizontalScrollbar.Height : 0);

            Panel.Size = new Vector2(panelWidth, panelHeight);

            UpdateScrollbars();
        }

        private void UpdateScrollbars()
        {
            if(HorizontalScrollbar != null)
            {
                HorizontalScrollbar.Control.Pivot = PivotPoint.TopLeft;
                HorizontalScrollbar.RelativePosition = Panel.RelativePosition + new Vector3(0, Panel.Height);
                HorizontalScrollbar.Width = Panel.Width;
                HorizontalScrollbar.Height = scrollbarWidth;
            }

            if (VerticalScrollbar != null)
            {
                VerticalScrollbar.Control.Pivot = PivotPoint.TopLeft;
                VerticalScrollbar.RelativePosition = Panel.RelativePosition + new Vector3(Panel.Width, 0);
                VerticalScrollbar.Height = Panel.Height;
                VerticalScrollbar.Width = scrollbarWidth;
            }
        }

        private void AddHorizontalScrollbar()
        {
            if (HorizontalScrollbar != null)
            {
                RemoveHorizontalScrollbar();
            }

            HorizontalScrollbar = new ScrollbarControl(this.Name + "HScrollbar");
            HorizontalScrollbar.Anchor = Anchor.Bottom | Anchor.Left | Anchor.Right;
            HorizontalScrollbar.Orientation = Orientation.Horizontal;
            HorizontalScrollbar.Width = Panel.Width;
            HorizontalScrollbar.Height = scrollbarWidth;

            if (Panel.Parent != null)
            {
                Panel.Parent.AttachControl(HorizontalScrollbar.Control);
            }

            Panel.HorizontalScrollbar = HorizontalScrollbar.Control;
        }

        private void AddVerticalScrollbar()
        {
            if (VerticalScrollbar != null)
            {
                RemoveVerticalScrollbar();
            }

            VerticalScrollbar = new ScrollbarControl(this.Name + "VScrollbar");
            VerticalScrollbar.Anchor = Anchor.Right | Anchor.Top | Anchor.Bottom;
            VerticalScrollbar.Orientation = Orientation.Vertical;
            VerticalScrollbar.Height = Panel.Height;
            VerticalScrollbar.Width = scrollbarWidth;

            if (Panel.Parent != null)
            {
                Panel.AttachControl(VerticalScrollbar.Control);
            }

            Panel.VerticalScrollbar = VerticalScrollbar.Control;
        }

        private void RemoveHorizontalScrollbar()
        {
            Panel.HorizontalScrollbar = null;

            if (HorizontalScrollbar != null)
            {
                Panel.Height += HorizontalScrollbar.Height;

                HorizontalScrollbar.Dispose();

                GameObject.DestroyImmediate(HorizontalScrollbar.Control.GameObject);
                HorizontalScrollbar = null;
            }
        }

        private void RemoveVerticalScrollbar()
        {
            Panel.HorizontalScrollbar = null;

            if (VerticalScrollbar != null)
            {
                Panel.Height += VerticalScrollbar.Width;

                VerticalScrollbar.Dispose();

                GameObject.DestroyImmediate(VerticalScrollbar.Control.GameObject);
                VerticalScrollbar = null;
            }
        }

        public override void AttachTo(GameObject gameObject)
        {
            Panel.GameObject.transform.parent = gameObject.transform;

            if (HorizontalScrollbar != null)
            {
                HorizontalScrollbar.AttachTo(gameObject);
            }

            if (VerticalScrollbar != null)
            {
                VerticalScrollbar.AttachTo(gameObject);
            }
        }

        public override void AttachTo(IColossalControl control)
        {
            control.AttachControl(Panel);

            if (HorizontalScrollbar != null)
            {
                HorizontalScrollbar.AttachTo(control);
            }

            if (VerticalScrollbar != null)
            {
                VerticalScrollbar.AttachTo(control);
            }
        }

        public override void Dispose()
        {
            Panel.Dispose();

            if (HorizontalScrollbar != null)
            {
                HorizontalScrollbar.Dispose();
            }

            if (VerticalScrollbar != null)
            {
                VerticalScrollbar.Dispose();
            }
        }
    }
}
