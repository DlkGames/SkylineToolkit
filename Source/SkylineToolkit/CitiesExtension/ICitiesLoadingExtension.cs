using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.CitiesExtension
{
    public interface ICitiesLoadingExtension : ILoadingExtension, ICitiesExtension
    {
        ILoading LoadingManager { get; }

        string CurrentTheme { get; set; }

        bool LoadingComplete { get; }

        AppMode CurrentMode { get; }
    }
}
