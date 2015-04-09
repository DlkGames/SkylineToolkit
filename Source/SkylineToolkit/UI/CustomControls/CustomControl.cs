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
