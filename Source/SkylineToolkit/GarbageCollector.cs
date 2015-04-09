using SkylineToolkit.UI;
using System;
using System.Collections.Generic;

namespace SkylineToolkit
{
    public static class GarbageCollector
    {
        private static IList<IDisposableControl> markedControls;

        static GarbageCollector()
        {
            markedControls = new List<IDisposableControl>();
        }

        public static void MarkControlForCollection(IDisposableControl control)
        {
            if (markedControls == null)
            {
                return;
            }

            if (!markedControls.Contains(control))
            {
                markedControls.Add(control);
            }
        }

        public static void CollectMarkedControls()
        {
            foreach(IDisposableControl control in markedControls)
            {
                // Force ColossalControls to unsubscribe from it's events
                if (control is IColossalControl)
                {
                    ((IColossalControl)control).UnsubscribeFromEvents();
                }

                control.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
