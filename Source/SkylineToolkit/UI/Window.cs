using SkylineToolkit.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public static class Window
    {
        public static WindowControl Create(string name)
        {
            GameObject go = new GameObject(name);

            go.transform.parent = ColossalControl.ColossalUIView.transform;

            WindowControl window = go.AddComponent<WindowControl>();

            return window;
        }
    }
}
