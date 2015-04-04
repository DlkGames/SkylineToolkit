

#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public struct Keyframe
    {
        public float time;
        public float value;

        public float Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }

        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public Keyframe(float time, float value)
        {
            this.time = time;
            this.value = value;
        }
    }
}
