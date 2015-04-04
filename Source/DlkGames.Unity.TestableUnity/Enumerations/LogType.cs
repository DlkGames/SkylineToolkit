#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public enum LogType
    {
        Error,
        Assert,
        Warning,
        Log,
        Exception
    }
}