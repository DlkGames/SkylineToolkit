using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public interface IOptionsProvider : IDisposable
    {
        /// <summary>
        /// Gets or sets whether the provider should reload the settings when the data storage changes.
        /// E.g. the settings file get's manipulated by the user.
        /// </summary>
        bool EnableAutoReload
        {
            get;
            set;
        }

        IModOptions ModOptions { get; }

        void Save();

        void Reload();
    }
}
