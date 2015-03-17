using UnityEngine;
namespace SkylineToolkit.UI
{
    public interface IControl
    {
        GameObject GameObject { get; }

        bool IsActive { get; set; }
    }
}
