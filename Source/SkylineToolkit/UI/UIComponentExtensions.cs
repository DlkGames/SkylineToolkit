﻿using ColossalFramework.UI;
using SkylineToolkit.UI.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public static class UIComponentExtensions
    {
        public static ColossalControl ToSkylineToolkitControl(this UIComponent component)
        {
            return ColossalControl.FromUIComponent(component);
        }

        public static ColossalControl<T> ToSkylineToolkitControl<T>(this UIComponent component)
            where T : UIComponent
        {
            return (ColossalControl<T>)ColossalControl<T>.FromUIComponent(component);
        }
    }
}
