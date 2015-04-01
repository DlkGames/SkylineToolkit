using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Events
{
    public class CancellableEventArgs : SkylineEventArgs
    {
        private bool cancel = false;

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

    }
}
