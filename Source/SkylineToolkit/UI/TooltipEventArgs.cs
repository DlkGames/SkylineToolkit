using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public class TooltipEventArgs : ControlEventArgs
    {
        public TooltipEventArgs(UITooltipEventParameter originalParams)
            : base(originalParams)
        {
            this.TooltipControl = ColossalControl<UIComponent>.FromUIComponent(originalParams.tooltip);
        }

        public TooltipEventArgs(IColossalControl tooltipControl)
        {
            this.TooltipControl = tooltipControl;
        }

        public IColossalControl TooltipControl { get; protected set; }
    }
}
