using System;
using System.Linq;


#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public class AnimationCurve
    {
        public Keyframe[] Keys
        {
            get;
            set;
        }

        public AnimationCurve()
        {

        }

        public AnimationCurve(Keyframe key, params Keyframe[] keys)
        {
            var keys2 = keys.ToList();
            keys2.Add(key);

            this.Keys = keys2.ToArray();
        }

        public AnimationCurve(Keyframe[] keys)
        {
            this.Keys = keys;
        }

        public float Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
