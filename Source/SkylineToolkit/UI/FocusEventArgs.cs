using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public class FocusEventArgs : ControlEventArgs
    {
        public FocusEventArgs(UIFocusEventParameter originalParams)
            : base(originalParams)
        {
            this.FocusedControl = ColossalControl<UIComponent>.FromUIComponent(originalParams.gotFocus);
            this.LostFocusControl = ColossalControl<UIComponent>.FromUIComponent(originalParams.lostFocus);
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
