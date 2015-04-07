using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public abstract class ControlsContainer : IControlsContainer
    {
        public abstract void Dispose();

        protected void OnDestroy()
        {
            this.Dispose();
        }

        ~ControlsContainer()
        {
            this.Dispose();
        }

        public abstract void AttachTo(GameObject gameObject);

        public abstract void AttachTo(IColossalControl control);
    }
}
