using ColossalFramework.UI;
using System;
namespace SkylineToolkit.UI
{
    public interface IColossalControl : IControl
    {
        UIComponent UIComponent { get; }

        void InitializeComponent(string name);
    }
}
