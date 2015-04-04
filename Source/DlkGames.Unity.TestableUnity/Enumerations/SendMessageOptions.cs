#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public enum SendMessageOptions
    {
        RequireReceiver,
        DontRequireReceiver
    }
}