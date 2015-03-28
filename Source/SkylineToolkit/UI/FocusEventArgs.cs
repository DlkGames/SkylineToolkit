using ColossalFramework.UI;

namespace SkylineToolkit.UI
{
    public class FocusEventArgs : ControlEventArgs
    {
        public FocusEventArgs(UIFocusEventParameter originalParams)
            : base(originalParams)
        {
            this.FocusedControl = ColossalControl.FromUIComponent(originalParams.gotFocus);
            this.LostFocusControl = ColossalControl.FromUIComponent(originalParams.lostFocus);
        }

        public FocusEventArgs(IColossalControl focusedControl, IColossalControl lostFocusControl)
        {
            this.FocusedControl = focusedControl;
            this.LostFocusControl = lostFocusControl;
        }

        public IColossalControl FocusedControl
        {
            get;
            private set;
        }

        public IColossalControl LostFocusControl
        {
            get;
            private set;
        }
    }
}
