using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    /// <summary>
    /// Represents a bezier path consisting of Vector3 vectors from the Unity engine.
    /// </summary>
    [Serializable]
    public class BezierPath : IEnumerable<Bezier3>, IEnumerable<Vector3>
    {
        #region Unity Engine Inspector Properties
        /// <summary>
        /// All points in this bezier path.
        /// </summary>
        public List<Vector3> points = new List<Vector3>();
        #endregion

        #region Private fields
        private float minSquareDistanceCache = 0.01f;
        private float divisionThresholdCache = -0.99f;
        #endregion

        #region Properties
        /// <summary>
        /// All points in this bezier path.
        /// </summary>
        public Vector3[] Points
        {
            get
            {
                return this.points.ToArray();
            }
            private set
            {
                if (value.Length < 4 || (value.Length - 1) % 3 != 0)
                {
                
                }

                this.points = value.ToList();
            }
        }

        /// <summary>
        /// The main bezier start/end points in this bezier path.
        /// </summary>
        public Vector3[] BezierPoints
        {
            get
            {
                if (this.points.Count < 4)
                {
                    return new Vector3[0];
                }

                return this.points.Where((vec, index) => index == 0 || index % 3 == 0).ToArray();
            }
        }

        /// <summary>
        /// The handle control points in this bezier path.
        /// </summary>
        public Vector3[] HandlePoints
        {
            get
            {
                if (this.points.Count < 4)
                {
                    return new Vector3[0];
                }

                return this.points.Where((vec, index) => index != 0 && index % 3 != 0).ToArray();
            }
        }

        /// <summary>
        /// The beziers of which the bezier path consists.
        /// </summary>
        public Bezier3[] Beziers
        {
            get
            {
                if (this.points.Count < 4)
                {
                    return new Bezier3[0];
                }

                Vector3[] bezierPoints = this.BezierPoints;
                Vector3[] handlePoints = this.HandlePoints;

                return this.BezierPoints
                    .Take(bezierPoints.Length - 1)
                    .Select((bp, index) => new Bezier3(bp, handlePoints[2 * index], handlePoints[2 * index + 1], bezierPoints[index + 1]))
                    .ToArray();
            }
        }

        /// <summary>
        /// The total count of points in this bezier path.
        /// </summary>
        public int PointsCount
        {
            get
            {
                if (this.points.Count == 0)
                {
                    return 0;
                }

                return this.BezierCount * 3 + 1;
            }
        }

        /// <summary>
        /// The count of beziers in this bezier path.
        /// </summary>
        public int BezierCount
        {
            get
            {
                if (this.points.Count < 0)
                {
                    return 0;
                }

                return (this.points.Count - 1) / 3;
            }
        }
        /// <summary>
        /// The count of handle points in this bezier path.
        /// </summary>
        public int HandleCount
        {
            get
            {
                if (this.points.Count < 0)
                {
                    return 0;
                }

                return PointsCount - this.BezierCount;
            }
        }
        #endregion

        #region API
        /// <summary>
        /// Returns the bezier at the requested <paramref name="index"/> from the bezier path.
        /// </summary>
        /// <param name="index">The index of the bezier.</param>
        /// <returns>The bezier at the requested index.</returns>
        public Bezier3 GetBezierAt(int index)
        {
            if (index > this.BezierCount - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return new Bezier3(this.points[3 * index], this.points[3 * index + 1], this.points[3 * index + 2], this.points[3 * index + 3]);
        }

        /// <summary>
        /// Replaces the bezier at the given <paramref name="index"/> with the new <paramref name="bezier"/>.
        /// </summary>
        /// <param name="index">Index at which to replace the bezier.</param>
        /// <param name="bezier">Bezier to insert at the index.</param>
        public void SetBezierAt(int index, Bezier3 bezier)
        {
            if (index > this.BezierCount - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            this.points[3 * index] = bezier.p0;
            this.points[3 * index + 1] = bezier.p1;
            this.points[3 * index + 2] = bezier.p2;
            this.points[3 * index + 3] = bezier.p3;
        }

        /// <summary>
        /// Moves a bezier point with the given <paramref name="movement"/> offset.
        /// </summary>
        /// <param name="index">The index of the handle point to be moved.</param>
        /// <param name="movement">The offset to move the point.</param>
        public void MoveBezierPoint(int index, Vector3 movement)
        {
            if (index > this.BezierPoints.Length - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            index = 3 * index;

            this.points[index] += movement;

            if (index != 0)
            {
                this.points[index - 1] += movement;
            }

            if (index != this.PointsCount - 1)
            {
                this.points[index + 1] += movement;
            }
        }

        /// <summary>
        /// Moves a handle point with the given <paramref name="movement"/> offset. This will move the corresponding
        /// handle point at the other side of the bezier point with a mirrored version of the
        /// <paramref name="movement"/> offset.
        /// </summary>
        /// <param name="index">The index of the handle point to be moved.</param>
        /// <param name="movement">The offset to move the point(s).</param>
        public void MoveBezierHandlePoint(int index, Vector3 movement)
        {
            this.MoveBezierHandlePoint(index, movement, false);
        }

        /// <summary>
        /// Moves a handle point with the given <paramref name="movement"/> offset. If <paramref name="splitted"/> is set,
        /// only the given point will be moved, otherwhise it will move the corresponding handle point at the other side
        /// of the bezier point with a mirrored version of the <paramref name="movement"/> offset.
        /// </summary>
        /// <param name="index">The index of the handle point to be moved.</param>
        /// <param name="movement">The offset to move the point(s).</param>
        /// <param name="splitted">Whether to move only the point at the given index, or to move both handle points for the bezier point.</param>
        public void MoveBezierHandlePoint(int index, Vector3 movement, bool splitted)
        {
            if (index > this.HandlePoints.Length - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int pointIndex;

            if (index == 0)
            {
                pointIndex = 1;
            }
            else
            {
                pointIndex = Mathf.FloorToInt(index / 2) + index + 1;
            }

            this.points[pointIndex] += movement;

            if (!splitted && index != 0 && pointIndex != this.PointsCount - 2)
            {
                if (index % 2 == 0)
                {
                    this.points[pointIndex - 2] = this.points[pointIndex - 1] + (this.points[pointIndex] - this.points[pointIndex - 1]) * -1;
                }
                else
                {
                    this.points[pointIndex + 2] = this.points[pointIndex + 1] + (this.points[pointIndex] - this.points[pointIndex + 1]) * -1;
                }
            }
        }

        /// <summary>
        /// Inserts the given <paramref name="point"/> at the <paramref name="index"/> inside the
        /// bezier path's points collection.
        /// Note: Using this method could lead to undesired behaviours in working with the bezier path.
        /// </summary>
        /// <param name="index">The index at which to insert the point.</param>
        /// <param name="point">The point to be inserted.</param>
        public void InsertPoint(int index, Vector3 point)
        {
            this.points.Insert(index, point);
        }

        /// <summary>
        /// Adds a new blank bezier straight forward from the last point of the bezier path.
        /// The bezier end point will be 3 units a in the direction from the last point.
        /// </summary>
        public void AddBezier()
        {
            if (this.points.Count == 0)
            {
                this.points.Add(Vector3.zero);
                this.points.Add(Vector3.up);
            }

            Vector3 lastPoint = this.points[this.points.Count - 1];

            Vector3 direction = (lastPoint - this.points[this.points.Count - 2]).normalized;

            this.points.AddRange(new Vector3[] {
                lastPoint + direction,
                lastPoint + 2 * direction,
                lastPoint + 3 * direction
            });
        }

        /// <summary>
        /// Adds a new bezier to the bezier path with the given <paramref name="endPoint"/> as the last point of
        /// the bezier. The control handle points for the newly created bezier will be interpolated automatically.
        /// </summary>
        /// <param name="endPoint">The endpoint of the new bezier to add.</param>
        public void AddBezier(Vector3 endPoint)
        {
            if (this.points.Count == 0)
            {
                this.points.Add(Vector3.zero);
            }

            Vector3 lastPoint = this.points[this.points.Count - 1];

            Vector3 firstControlPoint;

            if (this.points.Count == 1)
            {
                firstControlPoint = lastPoint + (endPoint - lastPoint).normalized;
            }
            else
            {
                firstControlPoint = lastPoint + ((this.points[this.points.Count - 2] - lastPoint) * -1);
            }

            this.points.AddRange(new Vector3[] {
                firstControlPoint,
                endPoint - (endPoint - firstControlPoint).normalized,
                endPoint
            });
        }

        /// <summary>
        /// Adds the <paramref name="bezier"/> to the bezier path. The first point of the bezier will be ommited and
        /// the other three points will be directly attached to the end of the bezier path.
        /// </summary>
        /// <param name="bezier">The bezier to attach at the end of the bezier path.</param>
        public void AddBezier(Bezier3 bezier)
        {
            this.AddBezier(bezier, false);
        }

        /// <summary>
        /// Adds a <paramref name="bezier"/> to the end of the bezier path. If <paramref name="interpolateBetween"/> is
        /// set, the algorithm will create a new bezier between the actual bezier path and the <paramref name="bezier"/>
        /// to be added, otherwise the first point of the <paramref name="bezier"/> will be ommited and the other three 
        /// points will be directly attached to the end of the bezier path.
        /// </summary>
        /// <param name="bezier">Bezier to be attached at the bezier path.</param>
        /// <param name="interpolateBetween">Whether to create a connecting bezier between the bezier path and the added bezier.</param>
        public void AddBezier(Bezier3 bezier, bool interpolateBetween)
        {
            if (this.points.Count == 0)
            {
                this.points.AddRange(new Vector3[] {
                    bezier.p0,
                    bezier.p1,
                    bezier.p2,
                    bezier.p3
                });
                return;
            }

            if (interpolateBetween && Vector3.Distance(this.points[this.points.Count - 1], bezier.p0) > 0.05f)
            {
                Vector3 lastPoint = this.points[this.points.Count - 1];

                this.points.AddRange(new Vector3[] {
                    lastPoint + ((this.points[this.points.Count - 2] - lastPoint) * -1),
                    bezier.p0 + ((bezier.p1 - bezier.p0) * -1)
                });

                this.points.Add(bezier.p0);
            }

            this.points.AddRange(new Vector3[] {
                bezier.p1,
                bezier.p2,
                bezier.p3
            });
        }

        /// <summary>
        /// Adds a new <paramref name="bezier"/> at the given <paramref name="index"/> to the bezier path.
        /// </summary>
        /// <param name="index">The index at which to insert the new <paramref name="bezier"/>.</param>
        /// <param name="bezier">The bezier to insert at the given <paramref name="index"/>.</param>
        public void InsertBezier(int index, Bezier3 bezier)
        {
            if (index > this.BezierCount - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int pointIndex = 3 * index;

            this.points.InsertRange(pointIndex, new Vector3[] {
                bezier.p0, bezier.p1, bezier.p2
            });
        }

        /// <summary>
        /// Removes the bezier at the given <paramref name="index"/> from the bezier path.
        /// </summary>
        /// <param name="index">The index of the bezier path to be removed.</param>
        public void RemoveBezier(int index)
        {
            if (index > this.BezierCount - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int pointIndex = 3 * index;

            this.points.RemoveRange(pointIndex, 3);
        }

        /// <summary>
        /// Reduces the number of bezier points in the bezierpath and adjusts the corresponding control
        /// handle points to generate a mostly accurate curve to the original one.
        /// </summary>
        /// <param name="minSqrDistance">Minimum square distance between two points.</param>
        /// <param name="maxSqrDistance">Maximum square distance between two points.</param>
        public void Optimize(float minSqrDistance, float maxSqrDistance)
        {
            if (this.points.Count < 4 || (this.points.Count - 1) % 3 != 0)
            {
                return;
            }

            Stack<Vector3> newPoints = new Stack<Vector3>();
            Vector3[] bezierPoints = this.BezierPoints;

            newPoints.Push(bezierPoints[0]);

            Vector3 potentialNewPoint = bezierPoints[1];

            for (int i = 2; i < bezierPoints.Length; i++)
            {
                if (((potentialNewPoint - bezierPoints[i]).sqrMagnitude > minSqrDistance)
                    && ((newPoints.Peek() - bezierPoints[i]).sqrMagnitude < maxSqrDistance))
                {
                    newPoints.Push(potentialNewPoint);
                }

                potentialNewPoint = bezierPoints[i];
            }

            // Correct last few points of the bezier path
            Vector3 p1 = newPoints.Pop();
            Vector3 p0 = newPoints.Peek();
            Vector3 tangent = (p0 - potentialNewPoint).normalized;

            float d2 = (potentialNewPoint - p1).magnitude;
            float d1 = (p1 - p0).magnitude;

            p1 = p1 + tangent * ((d1 - d2) / 2);

            newPoints.Push(p1);
            newPoints.Push(potentialNewPoint);

            this.Points = BezierPath.Interpolate(newPoints.ToArray()).Points.ToArray();
        }

        /// <summary>
        /// Finds drawing points for the bezier path. Each bezier in the path will be split into the given number of 
        /// <paramref name="segmentsPerBezier"/>.
        /// </summary>
        /// <param name="segmentsPerBezier">Count of segments to split each bezier contained in the bezier path into.</param>
        /// <returns>A collection consisting of evaluated drawing points for the bezier path.</returns>
        public IEnumerable<Vector3> GetDrawingPoints(int segmentsPerBezier)
        {
            IList<Vector3> drawingPoints = new List<Vector3>();

            Bezier3[] beziers = this.Beziers;

            for (int i = 0; i < this.BezierCount; i++)
            {
                Bezier3 bezier = beziers[i];

                if (i == 0)
                {
                    drawingPoints.Add(bezier.GetPointAtTime(0));
                }

                for (int j = 1; j <= segmentsPerBezier; j++)
                {
                    float t = j / (float)segmentsPerBezier;

                    drawingPoints.Add(bezier.GetPointAtTime(t));
                }
            }

            return drawingPoints.AsEnumerable();
        }

        /// <summary>
        /// Recursively finds drawing points for the bezier path.
        /// </summary>
        /// <param name="minSquareDistance">The minimum square distance between two drawing points.</param>
        /// <param name="divisionThreshold">The angle threshold below which the bezier will be split into multiple drawing points.</param>
        /// <returns>A collection consisting of recursively evaluated drawing points for the bezier path.</returns>
        public IEnumerable<Vector3> GetDrawingPoints(float minSquareDistance, float divisionThreshold)
        {
            List<Vector3> drawingPoints = new List<Vector3>();

            Bezier3[] beziers = this.Beziers;

            for (int i = 0; i < this.BezierCount; i++)
            {
                Bezier3 bezier = beziers[i];

                IList<Vector3> bezierCurveDrawingPoints = this.FindDrawingPoints(bezier, minSquareDistance, divisionThreshold).ToList();

                if (i != 0)
                {
                    // Remove the fist point, as it it the same as the last point of the previous bezier.
                    bezierCurveDrawingPoints.RemoveAt(0);
                }

                drawingPoints.AddRange(bezierCurveDrawingPoints);
            }

            return drawingPoints.AsEnumerable();
        }
        #endregion

        #region Internal API
        /// <summary>
        /// Recursively finds drawing points for the <paramref name="bezier"/>.
        /// </summary>
        /// <param name="bezier">The bezier to search points on.</param>
        /// <param name="minSquareDistance">The minimum square distance between two drawing points.</param>
        /// <param name="divisionThreshold">The angle threshold below which the bezier will be split into multiple drawing points.</param>
        /// <returns>A collection consisting of recursively evaluated drawing points for the <paramref name="bezier"/>.</returns>
        private IEnumerable<Vector3> FindDrawingPoints(Bezier3 bezier, float minSquareDistance, float divisionThreshold)
        {
            List<Vector3> pointList = new List<Vector3>();

            pointList.Add(bezier.GetPointAtTime(0));
            pointList.Add(bezier.GetPointAtTime(1));

            this.minSquareDistanceCache = minSquareDistance;
            this.divisionThresholdCache = divisionThreshold;

            this.FindDrawingPoints(bezier, 0, 1, pointList, 1);

            return pointList;
        }

        /// <summary>
        /// Recursively finds drawing points for the <paramref name="bezier"/> in the segment between the start time <paramref name="t0"/>
        /// and the end time <paramref name="t1"/> on the <paramref name="bezier"/>. The newly evaluated drawing points will be added to the
        /// given <paramref name="pointList"/> at the <paramref name="insertionIndex"/>.
        /// This method uses the private divisionThresholdCache variable to check whether another recursive call to find a drawing point is neccessary.
        /// The private minSquareDistanceCache variable will be used to check whether the start and end time points are at least away that distance
        /// from each other.
        /// </summary>
        /// <param name="bezier">The bezier to search points on.</param>
        /// <param name="t0">The start time for the drawing point search on the bezier curve.</param>
        /// <param name="t1">The end time for the drawing point search on the bezier curve.</param>
        /// <param name="pointList">The list of already evaluated drawing points.</param>
        /// <param name="insertionIndex">The index at which the newly evaluated drawing points would be inserted into the <paramref name="pointList"/>.</param>
        /// <returns>The number of found drawing points.</returns>
        private int FindDrawingPoints(Bezier3 bezier, float t0, float t1, List<Vector3> pointList, int insertionIndex)
        {
            Vector3 left = bezier.GetPointAtTime(t0);
            Vector3 right = bezier.GetPointAtTime(t1);

            if ((left - right).sqrMagnitude < this.minSquareDistanceCache)
            {
                return 0;
            }

            float tMid = (t0 + t1) / 2;
            Vector3 mid = bezier.GetPointAtTime(tMid);

            Vector3 leftDirection = (left - mid).normalized;
            Vector3 rightDirection = (right - mid).normalized;

            if (Vector3.Dot(leftDirection, rightDirection) > this.divisionThresholdCache || Mathf.Abs(tMid - 0.5f) < 0.0001f)
            {
                int pointsAddedCount = 0;

                pointsAddedCount += this.FindDrawingPoints(bezier, t0, tMid, pointList, insertionIndex);
                pointList.Insert(insertionIndex + pointsAddedCount, mid);
                pointsAddedCount++;
                pointsAddedCount += this.FindDrawingPoints(bezier, tMid, t1, pointList, insertionIndex + pointsAddedCount);

                return pointsAddedCount;
            }

            return 0;
        }
        #endregion

        #region Factory
        /// <summary>
        /// Creates a new bezier path along the given waypoints. The algorithm will automatically create new control points
        /// between the given <paramref name="points"/> to guarantee a smooth curve along the path.
        /// </summary>
        /// <param name="points">The waypoints to use as bezier points for the newly created bezier path.</param>
        /// <returns>A bezier path through the given bezier points.</returns>
        public static BezierPath Interpolate(Vector3[] points)
        {
            if (points.Length < 2)
            {
                throw new ArgumentOutOfRangeException("There are at least two points required to interpolate a bezier path.", "points");
            }

            BezierPath path = new BezierPath();

            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                {
                    Vector3 p1 = points[i];
                    Vector3 p2 = points[i + 1];

                    Vector3 tangent = (p2 - p1);
                    Vector3 q1 = p1 + tangent;

                    path.points.Add(p1);
                    path.points.Add(q1);
                }
                else if (i == points.Length - 1)
                {
                    Vector3 p0 = points[i - 1];
                    Vector3 p1 = points[i];
                    Vector3 tangent = (p1 - p0);
                    Vector3 q0 = p1 - tangent;

                    path.points.Add(q0);
                    path.points.Add(p1);
                }
                else
                {
                    Vector3 p0 = points[i - 1];
                    Vector3 p1 = points[i];
                    Vector3 p2 = points[i + 1];
                    Vector3 tangent = (p2 - p0).normalized;
                    Vector3 q0 = p1 - tangent * (p1 - p0).magnitude;
                    Vector3 q1 = p1 + tangent * (p2 - p1).magnitude;

                    path.points.Add(q0);
                    path.points.Add(p1);
                    path.points.Add(q1);
                }
            }

            return path;
        }
        #endregion

        #region IEnumerable implementation
        public IEnumerator<Bezier3> GetEnumerator()
        {
            for (int i = 0; i < this.BezierCount; i++)
            {
                yield return this.GetBezierAt(i);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
        {
            for (int i = 0; i < this.PointsCount; i++)
            {
                yield return this.points[i];
            }
        }
        #endregion
    }
}
