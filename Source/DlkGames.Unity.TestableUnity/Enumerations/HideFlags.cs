using System;

#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    [Flags]
    public enum HideFlags
    {
        HideInHierarchy = 1,
        HideInInspector = 2,
        DontSave = 4,
        NotEditable = 8,
        HideAndDontSave = NotEditable | DontSave | HideInHierarchy,
    }
}