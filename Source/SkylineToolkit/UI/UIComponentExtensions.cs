using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public static class UIComponentExtensions
    {
        public static IColossalControl ToSkylineToolkitControl(this UIComponent component)
        {
            return ColossalControl<UIComponent>.FromUIComponent(component);
        }

        public static ColossalControl<T> ToSkylineToolkitControl<T>(this UIComponent component)
            where T : UIComponent
        {
            return (ColossalControl<T>)ColossalControl<T>.FromUIComponent(component);
        }
    }
}
