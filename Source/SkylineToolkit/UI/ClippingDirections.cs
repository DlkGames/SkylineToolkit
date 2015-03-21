using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    [Flags]
    public enum ClippingDirections
    {
        Top = 1,
        Bottom = 2,
        Right = 4,
        Left = 8
    }
}
