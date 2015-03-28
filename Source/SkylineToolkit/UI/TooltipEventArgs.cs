using ColossalFramework.UI;

namespace SkylineToolkit.UI
{
    public class TooltipEventArgs : ControlEventArgs
    {
        public TooltipEventArgs(UITooltipEventParameter originalParams)
            : base(originalParams)
        {
            this.TooltipControl = ColossalControl.FromUIComponent(originalParams.tooltip);
        }

        public TooltipEventArgs(IColossalControl tooltipControl)
        {
            this.TooltipControl = tooltipControl;
        }

        public IColossalControl TooltipControl { get; protected set; }
    }
}
