using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    [Flags]
    public enum PositionAnchor
    {
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        All = Right | Left | Bottom | Top,
        CenterHorizontal = 64,
        CenterVertical = 128,
        Proportional = 256,
        None = 0,
    }
}
