﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    // Basic class handling geometric transformations
    //
    public class PointList : IEnumerable<Vector3> {

        //
        // Points are explicitely represented
        //
        private List<Vector3> m_points = new List<Vector3>();

        //
        // UV's are implicitly represented (default is (0,0) everywhere)
        //
        private IUVMapper m_uvMapper = new ZeroMapper();

        //
        // Keep a unique id for each unique point, share id when needed
        //
        private List<int> m_uids = new List<int>();

        //
        // Generator of unique identifiers
        //
        private static int m_uidCounter = 0;
        private static int NextUid() {
            return m_uidCounter++;
        }

        //
        // Add a new point
        //
        public void Add(Vector3 point) {
            m_points.Add (point);
            m_uids.Add (NextUid ());
        }

        public void Add(float x, float y, float z) {
            m_points.Add (new Vector3(x, y, z));
            m_uids.Add (NextUid ());
        }

        //
        // Copy a point from another PointList and preserve point uid, see Bridge() method 
        //
        public void Copy(PointList list, int index) {
            m_points.Add (list.m_points[index]);
            m_uids.Add (list.m_uids[index]);
        }

        //
        // Operator [] : point
        //
        public Vector3 this [int index] {
            get {
                return m_points [index];
            }
        }

        //
        // Uid () : uid
        //
        public int Uid(int index) {
            return m_uids [index];
        }

        //
        // Count
        //
        public int Count
        {
            get {
                return m_points.Count;
            }
        }

        //
        // Enumerator
        //
        public IEnumerator<Vector3> GetEnumerator() {
            return m_points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return m_points.GetEnumerator();
        }

        //
        // UV Mapper
        //
        public IUVMapper UVMapper 
        {
            get {
                return m_uvMapper;
            }
            set {
                m_uvMapper = value;
            }
        }

        //
        // Clear
        //
        public void Clear() {
            m_points.Clear ();
            m_uids.Clear ();
        }

        // ---------------------------------------------------------

        //
        // Barycenter
        //
        public Vector3 ComputeBarycenter() {
            Vector3 center = new Vector3 ();
            foreach (Vector3 point in this) {
                center += point;
            }
            return center*(float)(1f/this.Count);
        }

        //
        // Normal
        //
        public Vector3 ComputeNormal() {
            return Vector3.Cross(this[1]-this[0], this[2]-this[0]);
        }

        //
        // Reverse
        //
        public PointList Reverse() {
            PointList result = new PointList();
            for (int i=this.Count-1; i>=0; i--) {
                result.Add (this[i]);
            }
            return result;
        }

        //
        // Shift
        //
        public PointList Shift() {
            PointList result = new PointList();
            for (int i=0; i<this.Count; i++) {
                int index = (i + 1) % this.Count;
                result.Add (this[index]);
            }
            return result;
        }

        //
        // Translate
        //
        public PointList Translate(Vector3 direction) {
            PointList result = new PointList();
            foreach (Vector3 point in this) {
                result.Add (new Vector3 (point.x+direction.x, point.y+direction.y, point.z+direction.z));
            }
            return result;
        }

        //
        // Return sides
        //
        public  List<PointList> Bridge (PointList pointsB, bool close = false) {
            List<PointList> list = new List<PointList> ();

            // Add top points
            int iMax = this.Count;
            if (close == false) {
                iMax--;
            }

            for(int i=0; i<iMax; i++) {
                // Add side points
                PointList points = new PointList();
                points.Copy (this, i);
                points.Copy (this, (i+1) % this.Count);
                points.Copy (pointsB, (i+1) % this.Count);
                points.Copy (pointsB, i);
                list.Add (points);
            }

            return list;
        }


        //
        // Use origin, TODO : new origin should be returned !
        //
        public PointList Extrude(Vector3 origin, Vector3 direction, Vector3 nextDirection)
        {
            PointList result = new PointList();

            // Project origin
            Vector3 newOrigin = origin + direction;

            // Plane normal
            Vector3 planeNormal = Vector3.Lerp (direction.normalized, nextDirection.normalized, 0.5f);

            foreach (Vector3 point in this) {
                Vector3 newPoint = LinePlaneIntersection (point, direction, newOrigin, planeNormal);
                result.Add (newPoint);
            }

            return result;
        }

        //
        // Line / Plane intersection
        //
        private static Vector3 LinePlaneIntersection(Vector3 linePoint, Vector3 lineVec, Vector3 planePoint, Vector3 planeNormal) {

            Vector3 intersection = linePoint;

            // Compute distance between linePoint and the line-plane intersection point
            float dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
            float dotDenominator = Vector3.Dot(lineVec.normalized, planeNormal);

            // line and plane are not parallel
            if(dotDenominator != 0.0f)
            {
                float length =  dotNumerator / dotDenominator;
                intersection = linePoint + length * lineVec.normalized;
            }

            return intersection;
        }

        //
        // 3 axis scale
        //
        public PointList Scale(Vector3 origin, float alpha)
        {
            PointList result = new PointList();

            foreach (Vector3 point in this) {
                Vector3 newPoint = alpha * point + (1f - alpha) * origin;
                result.Add (newPoint);
            }

            return result;
        }

        //
        // Uniform scale
        //
        public PointList Scale (float alpha) {
            return Scale(ComputeBarycenter(), alpha);
        }

        //
        // Only works for 4 sides
        //
        public List<PointList> Divide(int n, int m) {
            List<PointList> list = new List<PointList> ();

            if (this.Count != 4)
                return list;

            Vector3 vA = (this [1] - this [0])/n;
            Vector3 vB = (this [3] - this [0])/m;

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    PointList pointsA = new PointList();
                    pointsA.Add (this[0] + i*vA + j*vB);
                    pointsA.Add (this[0] + (i+1)*vA + j*vB);
                    pointsA.Add (this[0] + (i+1)*vA + (j+1)*vB);
                    pointsA.Add (this[0] + i*vA + (j+1)*vB);
                    list.Add(pointsA);
                }
            }

            return list;
        }

        //
        // alpha = 0 means object as is
        // alpha = 1 means sphere of given radius
        //
        public PointList Inflate(Vector3 center, float radius, float alpha) {
            PointList result = new PointList();

            foreach (Vector3 point in this) {
                Vector3 v = point - center;
                Vector3 spherePoint = center + radius * v.normalized;
                Vector3 newPoint = alpha * spherePoint + (1 - alpha) * point;
                result.Add (newPoint);
            }

            return result;
        }

        // ---------------------------------------------------------

        //
        //
        //
        public static PointList CreateUnitTile()
        {
            PointList result = new PointList ();
            result.Add (new Vector3(0.5f, 0, 0.5f));
            result.Add (new Vector3(0.5f, 0, -0.5f));
            result.Add (new Vector3(-0.5f, 0, -0.5f));
            result.Add (new Vector3(-0.5f, 0, 0.5f));
            return result;
        }

        //
        //
        //
        public static PointList CreateUnitPolygon(int sides)
        {
            PointList result = new PointList ();
            float indexToAngle = Mathf.PI*2f/(float)sides;
            for (int i = 0; i < sides; i++) {
                float angle = i * indexToAngle;
                result.Add (0.5f*Mathf.Sin(angle), 0f, 0.5f*Mathf.Cos(angle));
            }
            return result;
        }

        //
        //
        //
        public static PointList CreatePolygon(PointListAxis axis, float radius, int sides, float axisCoord = 0f)
        {
            PointList result = new PointList ();
            float indexToAngle = Mathf.PI*2f/(float)sides;
            switch(axis)
            {
            case PointListAxis.XAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (axisCoord, radius*Mathf.Cos(angle), radius*Mathf.Sin(angle));
                }
                break;

            case PointListAxis.YAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (radius*Mathf.Sin(angle), axisCoord, radius*Mathf.Cos(angle));
                }
                break;

            case PointListAxis.ZAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (radius*Mathf.Cos(angle), radius*Mathf.Sin(angle), axisCoord);
                }
                break;
            }
            return result;
        }
    }

}