using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.UI
{
    public class UIDiposingManager : IDisposable
    {
        private IList<IDisposableControl> controls = new List<IDisposableControl>();

        public IDisposableControl RegisterControl(IDisposableControl control)
        {
            this.controls.Add(control);

            return control;
        }

        public IDisposableControl R(IDisposableControl control)
        {
            return this.RegisterControl(control);
        }

        public T RegisterControl<T>(T control)
            where T : IDisposableControl
        {
            return (T)this.RegisterControl(control);
        }

        public T R<T>(T control)
            where T : IDisposableControl
        {
            return (T)this.RegisterControl(control);
        }

        public void Dispose()
        {
            foreach (IDisposableControl control in controls)
            {
                control.Dispose();
            }
        }
    }
}
