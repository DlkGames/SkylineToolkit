using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    /// <summary>
    /// Represents a cubic bézier curve (degree-two) in a 2D room in Unity.
    /// </summary>
    [System.Serializable]
    public class Bezier2 : System.Object
    {
        public Vector2 p0;
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;

        public float ti = 0f;

        private Vector2 b0 = Vector2.zero;
        private Vector2 b1 = Vector2.zero;
        private Vector2 b2 = Vector2.zero;
        private Vector2 b3 = Vector2.zero;

        private float Ax;
        private float Ay;

        private float Bx;
        private float By;

        private float Cx;
        private float Cy;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// <para>Handle1 = <paramref name="v0"/> + <paramref name="v1"/></para>
        /// <para>Handle2 = <paramref name="v3"/> + <paramref name="v2"/></para>
        /// </remarks>
        /// <param name="v0">First point.</param>
        /// <param name="v1">Handle of first point.</param>
        /// <param name="v2">Handle of second point.</param>
        /// <param name="v3">Second point.</param>
        public Bezier2(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            this.p0 = v0;
            this.p1 = v1;
            this.p2 = v2;
            this.p3 = v3;
        }

        /// <summary>
        /// Evaluates the point at the requested time on the bezier.
        /// </summary>
        /// <param name="t">Time from the beginning to the end of the bezier curve. Value form 0.0 to 1.0.</param>
        /// <returns>The vector of the point at the given time on the bezier.</returns>
        public Vector2 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);

            this.CheckConstant();

            float t2 = t * t;
            float t3 = t * t * t;

            float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;
            float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;

            return new Vector2(x, y);
        }

        private void SetConstant()
        {
            this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);
            this.Bx = 3f * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;
            this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;

            this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);
            this.By = 3f * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;
            this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;
        }

        // Check if p0, p1, p2 or p3 have changed
        private void CheckConstant()
        {
            if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3)
            {
                this.SetConstant();

                this.b0 = this.p0;
                this.b1 = this.p1;
                this.b2 = this.p2;
                this.b3 = this.p3;
            }
        }

        /// <summary>
        /// Evaluates the point at the requested time on the bezier defined by v0 to v3.
        /// </summary>
        /// <param name="t">Time from the beginning to the end of the bezier curve. Value form 0.0 to 1.0.</param>
        /// <param name="v0">First point.</param>
        /// <param name="v1">Handle of first point.</param>
        /// <param name="v2">Handle of second point.</param>
        /// <param name="v3">Second point.</param>
        /// <returns></returns>
        public static Vector2 CalculateBezierPoint(float t, Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            t = Mathf.Clamp01(t);

            float u = 1.0f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            // Terms
            Vector2 p = uuu * v0;
            p += 3 * uu * t * v1;
            p += 3 * u * tt * v2;
            p += ttt * v3;

            return p;
        }
    }
}
