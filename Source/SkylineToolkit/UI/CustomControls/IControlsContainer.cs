using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public interface IControlsContainer : IDisposableControl
    {
        void AttachTo(GameObject gameObject);

        void AttachTo(IColossalControl control);
    }
}
