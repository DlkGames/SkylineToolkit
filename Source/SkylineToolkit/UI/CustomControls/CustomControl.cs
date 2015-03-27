using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI.CustomControls
{
    public class CustomControl<T> : BaseControl, ICustomControl
        where T : UICustomControl
    {
    }
}
