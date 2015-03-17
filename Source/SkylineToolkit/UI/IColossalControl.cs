using ColossalFramework.UI;
namespace SkylineToolkit.UI
{
    public interface IColossalControl : IControl
    {
        UIComponent UIComponent { get; }

        void InitializeComponent(string name);
    }
}
