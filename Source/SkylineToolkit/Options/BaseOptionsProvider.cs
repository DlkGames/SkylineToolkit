using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public abstract class BaseOptionsProvider : IOptionsProvider
    {
        public BaseOptionsProvider(IModOptions modOptions)
        {
            this.ModOptions = modOptions;
        }

        public bool EnableAutoReload { get; set; }

        public IModOptions ModOptions { get; protected set; }

        public abstract void Save();

        public abstract void Reload();

        public virtual void Dispose()
        {
            this.Save();
        }
    }
}
