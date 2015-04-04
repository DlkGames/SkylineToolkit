using ColossalFramework.UI;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class MouseEventArgs : ControlEventArgs
    {
        public MouseEventArgs(UIMouseEventParameter originalParams)
            : base(originalParams)
        {
            this.Buttons = (MouseButtons)originalParams.buttons;
            this.Clicks = originalParams.clicks;
            this.Position = originalParams.position;
            this.MovementDelta = originalParams.moveDelta;
            this.Ray = originalParams.ray;
            this.MousewheelDelta = originalParams.wheelDelta;
        }

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

        public bool CheckButtonsPressed(MouseButtons mouseButtons)
        {
            return (this.Buttons & mouseButtons) == mouseButtons;
        }
    }
}
