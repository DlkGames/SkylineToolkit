using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public enum DragDropState
    {
        None,
        Dragging,
        Dropped,
        Denied,
        Cancelled,
        CancelledNoTarget
    }
}
