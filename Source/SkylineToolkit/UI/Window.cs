using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Window : Panel
    {
        public Window(string name, Rect rect)
            : base(name, rect)
        {
        }

        public Window(UIPanel panel)
            : base(panel)
        {
        }
    }
}
