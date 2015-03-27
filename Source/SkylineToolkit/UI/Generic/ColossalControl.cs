using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI.Generic
{
    public class ColossalControl<T> : Control, IColossalControl, IComparable<IColossalControl>
        where T : UIComponent
    {
    }
}
