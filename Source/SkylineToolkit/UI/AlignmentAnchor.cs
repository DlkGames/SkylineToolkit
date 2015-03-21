using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    [Flags]
    public enum AlignmentAnchor
    {
        TopRight = 0,
        TopLeft = 1,
        BottomRight = 2,
        BottomLeft = BottomRight | TopLeft
    }
}
