
namespace SkylineToolkit.Debugging
{
    public enum DebugConsoleMessageType
    {
        Normal = UnityEngine.LogType.Log,
        Warning = UnityEngine.LogType.Warning,
        Error = UnityEngine.LogType.Error,
        Exception = UnityEngine.LogType.Exception,
        Assert = UnityEngine.LogType.Assert,
        System = 20,
        Input = 21,
        Output = 22
    }
}
