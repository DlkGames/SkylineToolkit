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
            return Create(name, true);
        }

        public static WindowControl Create(string name, bool initiallyVisible)
        {
            Vector2 size = new Vector2(800, 600);
            Vector3 position = new Vector3((size.x / 2) * -1, (size.y / 2) * -1);

            return Create(name, initiallyVisible, size, position);
        }

        public static WindowControl Create(string name, bool initiallyVisible, Vector2 size)
        {
            Vector3 position = new Vector3((size.x / 2) * -1, (size.y / 2) * -1);
            
            return Create(name, initiallyVisible, size, position);
        }

        public static WindowControl Create(string name, bool initiallyVisible, Vector2 size, Vector3 position)
        {
            GameObject go = new GameObject(name);

            go.transform.parent = ColossalControl.ColossalUIView.transform;

            WindowControl window = go.AddComponent<WindowControl>();

            window.Position = position;
            window.Size = size;
            window.IsVisible = initiallyVisible;

            return window;
        }
    }
}
