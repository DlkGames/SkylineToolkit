using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    /// <summary>
    /// Represents a quadratic bézier curve (degree-two) in a 2D room in Unity.
    /// </summary>
    [System.Serializable]
    public class QuadraticBezier2 : System.Object
    {
        public Vector2 p0;
        public Vector2 p1;
        public Vector2 p2;

        public float ti = 0f;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">First point.</param>
        /// <param name="control">Handle point to control the curve.</param>
        /// <param name="end">Second point.</param>
        public QuadraticBezier2(Vector2 start, Vector2 control, Vector2 end)
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
        public Vector2 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);

            return QuadraticBezier2.CalculateBezierPoint(t, this.p0, this.p1, this.p2);

        }

        /// <summary>
        /// Evaluates the point at the requested time on the bezier defined by start to end controlled over control.
        /// </summary>
        /// <param name="t">Time from the beginning to the end of the bezier curve. Value form 0.0 to 1.0.</param>
        /// <param name="start">First point.</param>
        /// <param name="control">The control handle.</param>
        /// <param name="end">Second point.</param>
        /// <returns></returns>
        public static Vector2 CalculateBezierPoint(float t, Vector2 start, Vector2 control, Vector2 end)
        {
            return (((1 - t) * (1 - t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
        }
    }
}
