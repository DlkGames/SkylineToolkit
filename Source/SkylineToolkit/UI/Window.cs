using ColossalFramework.UI;
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
