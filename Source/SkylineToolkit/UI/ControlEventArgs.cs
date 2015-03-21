using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkylineToolkit.UI
{
    public abstract class ControlEventArgs : SkylineEventArgs
    {
        private UIComponentEventParameter originalParams;

        internal ControlEventArgs(UIComponentEventParameter originalParams)
        {
            this.originalParams = originalParams;
        }

        public ControlEventArgs()
        {
        }

        public bool Handled
        {
            get
            {
                return originalParams.used;
            }
            set
            {
                if (value)
                {
                    originalParams.Use();

                    return;
                }

                FieldInfo info = originalParams.GetType().GetField("used", BindingFlags.NonPublic | BindingFlags.SetField);

                if (info == null)
                {
                    return;
                }

                info.SetValue(originalParams, value);
            }
        }

        public UIComponentEventParameter OriginalParameters
        {
            get
            {
                return this.originalParams;
            }
        }
    }
}
