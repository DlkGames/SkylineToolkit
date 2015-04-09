using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public interface IModOptions
    {
        IOptionsProvider Provider { get; }

        IMod Mod { get; }

        void Save();

        void Reload();
    }
}
