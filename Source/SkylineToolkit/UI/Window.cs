using ColossalFramework.UI;
using SkylineToolkit.UI.CustomControls;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Window : BaseControl
    {
        private WindowControl control;

        public WindowControl Control
        {
            get
            {
                return this.control;
            }
            protected set
            {
                this.control = value;
            }
        }
    }
}
