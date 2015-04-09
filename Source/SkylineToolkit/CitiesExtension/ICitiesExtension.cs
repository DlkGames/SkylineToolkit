using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.CitiesExtension
{
    public interface ICitiesExtension
    {
        bool IsCreated { get; }

        IManagers Managers { get; }
    }
}
