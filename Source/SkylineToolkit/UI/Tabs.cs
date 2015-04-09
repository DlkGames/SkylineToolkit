using SkylineToolkit.UI.CustomControls;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public static class Tabs
    {
        public static TabsControl Create()
        {
            return new TabsControl();
        }

        public static TabsControl Create(string name)
        {
            return new TabsControl(name);
        }

        public static TabsControl Create(string name, Vector3 position, Vector2 size)
        {
            return new TabsControl(name, position, size);
        }
    }
}
