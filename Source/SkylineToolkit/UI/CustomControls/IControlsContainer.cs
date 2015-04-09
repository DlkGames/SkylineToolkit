using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public interface IControlsContainer : IDisposableControl
    {
        void AttachTo(GameObject gameObject);

        void AttachTo(IColossalControl control);
    }
}
