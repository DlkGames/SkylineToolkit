using System;

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
