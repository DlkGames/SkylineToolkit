using System;

#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public class Object
    {
        #region Private fields
        private bool _isNull = false;
        internal bool IsPrefab = true;

        internal ReferenceData ReferenceData;
        private string _name;
        private HideFlags _hideFlags;
        #endregion

        #region Properties
        public string name
        {
            get
            {
                AssertNull();
                return _name;
            }
            set
            {
                AssertNull();
                _name = value;
            }
        }

        public HideFlags hideFlags
        {
            get
            {
                AssertNull();
                return _hideFlags;
            }
            set
            {
                AssertNull();
                _hideFlags = value;
            }
        }
        #endregion

        #region API
        public int GetInstanceID()
        {
            AssertNull();

            return ReferenceData.InstanceID;
        }

        public override int GetHashCode()
        {
            return GetInstanceID();
        }
        #endregion

        #region Internal API
        /// <summary>
        /// Throws a new <see cref="System.ArgumentException"/> if <paramref name="arg"/> is null, with the specified message.
        /// </summary>
        /// <param name="arg">The argument to check</param>
        /// <param name="message">The message for the newly created ArgumentException</param>
        internal static void CheckNullArgument(Object arg, string message)
        {
            if (arg == null)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Checks whether the given Obejct is actually null or supposed to be null.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>Whether the given Object is actually null or supposed to be null.</returns>
        internal static bool IsNull(Object obj)
        {
            return ReferenceEquals(obj, null) || obj._isNull;
        }

        /// <summary>
        /// Throws a new <see cref="System.NullReferenceException"/> if this object is actually null or is supposed to be null.
        /// This is a replacement for Unity's null setting capabilities.
        /// </summary>
        internal void AssertNull()
        {
            if (_isNull)
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Sets the given Object to null when it's supposed to be null.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object to check</param>
        /// <returns>true, if the object was made null or already was null</returns>
        internal bool AssertReference<T>(ref T obj) where T : Object
        {
            if (ReferenceEquals(obj, null))
            {
                return true;
            }

            if (obj._isNull)
            {
                obj = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Combination of AssertReference and AssertNull.
        /// </summary>
        /// <typeparam name="T">The type of the Object</typeparam>
        /// <param name="obj">The object to check</param>
        internal void AssertNullRef<T>(ref T obj) where T : Object
        {
            if (_isNull)
            {
                obj = null;
            }

            throw new NullReferenceException();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Returns true if the object actually exists and is not null.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns></returns>
        public static implicit operator bool(Object obj)
        {
            return !ActuallyEquals(obj, null);
        }

        public static bool operator ==(Object x, Object y)
        {
            return ActuallyEquals(x, y);
        }

        public static bool operator !=(Object x, Object y)
        {
            return !ActuallyEquals(x, y);
        }
        #endregion

        #region Static API
        private static bool ActuallyEquals(Object lhs, Object rhs)
        {
            bool lhsIsNull = IsNull(lhs);
            bool rhsIsNull = IsNull(rhs);

            // Both are null, so they're the same thing
            if (lhsIsNull && rhsIsNull)
            {
                return true;
            }

            if (lhsIsNull)
            {
                return false;
            }

            if (rhsIsNull)
            {
                return false;
            }

            return ReferenceEquals(lhs, rhs);
        }

        /// <summary>
        /// Instantiate the specified object.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Object Instantiate(Object original)
        {
            CheckNullArgument(original, "The thing you want to instantiate is null.");

            // TODO: can the object be instantiated in space?

            throw new NotImplementedException();
        }

        /// <summary>
        /// Instantiate the specified object with the specified location and rotation.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
        {
            //TODO: check if the object is a prefab or an already existing object.
            //then either clone or instantiate

            throw new NotImplementedException();
        }

        /// <summary>
        /// Destroy the object at the end of the frame.
        /// </summary>
        /// <param name="obj">The object to be destroyed</param>
        public static void DestroyObject(Object obj)
        {
            DestroyObject(obj, 0.0f);
        }

        public static void DestroyImmediate(Object obj, bool allowDestroyingAssets)
        {
            obj._isNull = true;
        }

        public static void DestroyImmediate(Object obj)
        {
            DestroyImmediate(obj, false);
        }

        /// <summary>
        /// Destroy the object after the specified amount of time.
        /// </summary>
        /// <param name="obj">The object to be destroyed</param>
        /// <param name="t">The time after which to destroy the object</param>
        public static void DestroyObject(Object obj, float t)
        {
            //TODO: set isnull to true once destroyed

            throw new NotImplementedException();
        }
        #endregion

        #region API
        public override bool Equals(object o)
        {
            AssertNull();

            return ActuallyEquals(this, (Object)o);
        }

        public override string ToString()
        {
            AssertNull();

            return string.Format("Object {0} : {1}", _name, GetType());
        }
        #endregion
    }
}
