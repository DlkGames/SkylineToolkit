using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public abstract class ModOptions : IModOptions
    {
        private IOptionsProvider provider;

        private IMod mod;

        public ModOptions(IMod mod)
        {
            if (this.provider == null)
            {
                InitializeDefaultProvider();
            }
        }

        public IOptionsProvider Provider
        {
            get
            {
                if (this.provider == null)
                {
                    InitializeDefaultProvider();
                }

                return this.provider;
            }
            protected set
            {
                this.provider = value;
            }
        }

        public IMod Mod
        {
            get { return this.mod; }
            protected set { this.mod = value; }
        }

        protected virtual void InitializeDefaultProvider()
        {
            if (this.provider != null)
            {
                this.provider.Dispose();
                this.provider = null;
            }

            this.provider = new XmlOptionsProvider(this);
        }

        public virtual void Save()
        {
            if (this.mod == null || this.provider == null)
            {
                Log.Error("This mod options instance is invalid!");
                throw new InvalidOperationException("This mod options instance is invalid!");
            }

            this.Provider.Save();
        }

        public virtual void Reload()
        {
            this.Provider.Reload();
        }
    }
}
