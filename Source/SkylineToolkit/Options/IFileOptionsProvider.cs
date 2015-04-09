using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public interface IFileOptionsProvider : IOptionsProvider
    {
        string SettingsFile { get; set; }

        void SaveTo(Stream stream);

        void SaveTo(string file);

        void LoadFrom(string file);
    }
}
