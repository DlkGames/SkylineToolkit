
namespace SkylineToolkit.Debugging.Windows
{
    public interface IDebugConsoleWindow
    {
        void SetupWindow(int windowId, DebugConsole debugConsole);

        void DrawWindow(int windowId, DebugConsole debugConsole);
    }
}
