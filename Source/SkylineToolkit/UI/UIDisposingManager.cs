using System;
using System.Collections.Generic;

namespace SkylineToolkit.UI
{
    public class UIDisposingManager : IDisposable
    {
        private IList<IDisposableControl> controls = new List<IDisposableControl>();
        
        public IDisposableControl R(IDisposableControl control)
        {
            return this.RegisterControl(control);
        }

        public T RegisterControl<T>(T control)
            where T : IDisposableControl
        {
            this.controls.Add(control);

            return control;
        }

        public T R<T>(T control)
            where T : IDisposableControl
        {
            return (T)this.RegisterControl(control);
        }

        public void Dispose()
        {
            Log.Debug("DisposingManager", "Triggered disposing. Calling Dispose() for {0} controls.", controls.Count);

            foreach (IDisposableControl control in controls)
            {
                control.Dispose();
            }
        }
    }
}
