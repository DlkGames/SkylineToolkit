using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Events
{
    public delegate void PropChangedEventHandler<T>(object sender, PropChangedEventArgs<T> e);
}
