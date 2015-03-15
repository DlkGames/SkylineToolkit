using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    /// <summary>
    /// Represents a quadratic bézier curve (degree-two) in a 3D room in Unity.
    /// </summary>
    [System.Serializable]
    public class QuadraticBezier3 : System.Object
    {
        public Vector3 p0;
        public Vector3 p1;
        public Vector3 p2;

        public float ti = 0f;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">First point.</param>
        /// <param name="control">Handle point to control the curve.</param>
        /// <param name="end">Second point.</param>
        public QuadraticBezier3(Vector3 start, Vector3 control, Vector3 end)
        {
            this.p0 = start;
            this.p1 = control;
            this.p2 = end;
        }

        /// <summary>
        /// Evaluates the point at the requested time on the bezier.
        /// </summary>
        /// <param name="t">Time from the beginning to the end of the bezier curve. Value form 0.0 to 1.0.</param>
        /// <returns>The vector of the point at the given time on the bezier.</returns>
        public Vector3 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);

            return QuadraticBezier3.CalculateBezierPoint(t, this.p0, this.p1, this.p2);

        }

        /// <summary>
        /// Evaluates the point at the requested time on the bezier defined by start to end controlled over control.
        /// </summary>
        /// <param name="t">Time from the beginning to the end of the bezier curve. Value form 0.0 to 1.0.</param>
        /// <param name="start">First point.</param>
        /// <param name="control">The control handle.</param>
        /// <param name="end">Second point.</param>
        /// <returns></returns>
        public static Vector3 CalculateBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
        {
            return (((1 - t) * (1 - t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
        }
    }
}
