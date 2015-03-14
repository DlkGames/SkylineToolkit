using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(MouseButtons buttons, int clicks, Ray ray, Vector2 position, Vector2 movementDelta, float mousewheelDelta)
        {
            this.Buttons = buttons;
            this.Clicks = clicks;
            this.Position = position;
            this.MovementDelta = movementDelta;
            this.Ray = ray;
            this.MousewheelDelta = mousewheelDelta;
        }

        public MouseButtons Buttons { get; set; }

        public int Clicks { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 MovementDelta { get; set; }

        public Ray Ray { get; set; }

        public float MousewheelDelta { get; set; }
    }
}
