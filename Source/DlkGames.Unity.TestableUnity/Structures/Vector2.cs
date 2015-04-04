#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    /// <summary>
    /// 
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// 
        /// </summary>
        public float x;
        /// <summary>
        /// 
        /// </summary>
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
