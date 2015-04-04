using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public abstract class CustomControl : MonoBehaviour, ICustomControl
    {
        public abstract void Dispose();

        protected void OnDestroy()
        {
            this.Dispose();
        }
    }
}
