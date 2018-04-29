using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public enum AxisSide {Positive, Negative}

    //
    //
    //
    public class PrimitiveBuilder {


        //
        //
        //
        public static Mesh BuildCube(float size)
        {
            float halfSize = size * 0.5f;
            return BuildBox (-halfSize, halfSize, -halfSize, halfSize, -halfSize, halfSize);
        }


        //
        // Axis Aligned Box
        //
        public static Mesh BuildBox(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            PMesh builder = new PMesh();

            PointList pointsA = new PointList ();
            pointsA.Add (xMin, yMin, zMin);
            pointsA.Add (xMin, yMin, zMax);
            pointsA.Add (xMax, yMin, zMax);
            pointsA.Add (xMax, yMin, zMin);

            PointList pointsB = pointsA.Translate ( new Vector3(0f, yMax - yMin, 0f) );

            builder.Cap (pointsA.Reverse());
            builder.Cap (pointsA.Bridge (pointsB, true));
            builder.Cap (pointsB);

            return builder.Build();
        }


        //
        //
        //
        public static Mesh BuildSphere(PointListAxis axis, float radius, int sides)
        {
            if (radius > 0.02f) {
                
                // Works great up to radius = 0.02, under this limit normal issues appear

                PMesh builder = new PMesh ();

                PointList points = PointList.CreatePolygon (axis, radius, sides);

                int n = sides / 4;

                PointList[] pointsM = new PointList[n + 1];
                pointsM [0] = points;

                Vector3 v = Vector (axis);

                float indexToRadAngle = Mathf.PI * 0.5f / n;

                // First Hemisphere
                for (int i = 1; i <= n; i++) {
                    float angle = i * indexToRadAngle;
                    pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                    builder.Cap (pointsM [i - 1].Bridge (pointsM [i], true));
                }

                // Second Hemisphere
                for (int i = 1; i <= n; i++) {
                    float angle = -i * indexToRadAngle;
                    pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                    builder.Cap (pointsM [i].Bridge (pointsM [i - 1], true));
                }

                return builder.Build ();
            
            } else {
                
                Mesh mesh = BuildSphere (axis, 1.0f, sides);
                MeshAdapter.ScaleVertices (ref mesh, new Vector3 (radius, radius, radius));
                return mesh;
            }
        }


        //
        //
        //
        public static Mesh BuildHemisphere(PointListAxis axis, AxisSide axisSide, float radius, int sides)
        {
            if (radius > 0.02f) {

                // Works great up to radius = 0.02, under this limit normal issues appear

                PMesh builder = new PMesh ();

                PointList points = PointList.CreatePolygon (axis, radius, sides);

                int n = sides / 4;

                PointList[] pointsM = new PointList[n + 1];
                pointsM [0] = points;

                Vector3 v = Vector (axis);

                float indexToRadAngle = Mathf.PI * 0.5f / n;

                if (axisSide == AxisSide.Positive) {
                    
                    // First Hemisphere
                    for (int i = 1; i <= n; i++) {
                        float angle = i * indexToRadAngle;
                        pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                        builder.Cap (pointsM [i - 1].Bridge (pointsM [i], true));
                    }
                } else {

                    // Second Hemisphere
                    for (int i = 1; i <= n; i++) {
                        float angle = -i * indexToRadAngle;
                        pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                        builder.Cap (pointsM [i].Bridge (pointsM [i - 1], true));
                    }
                }

                return builder.Build ();

            } else {

                Mesh mesh = BuildHemisphere (axis, axisSide, 1.0f, sides);
                MeshAdapter.ScaleVertices (ref mesh, new Vector3 (radius, radius, radius));
                return mesh;
            }
        }


        //
        //
        //
        public static Mesh BuildUnitArcSphere(PointListAxis axis, float arcMinDeg, float arcMaxDeg, int sides)
        {
            PMesh builder = new PMesh ();

            int n = Mathf.FloorToInt((arcMaxDeg - arcMinDeg) * sides / 360f);

            PointList[] pointsM = new PointList[n + 1];

            for (int i = 0; i <= n; i++) {
                float angle = Mathf.Deg2Rad * (arcMinDeg + i * (arcMaxDeg - arcMinDeg) / n);
                pointsM[i] = PointList.CreatePolygon (axis, Mathf.Cos (angle), sides, Mathf.Sin (angle));
                if (i > 0) {
                    builder.Cap (pointsM [i-1].Bridge (pointsM [i], true));
                }
            }

            //float epsilonDegree = 0.1f;

            //if (arcMinDeg > -90f) {
            builder.Cap (pointsM [0].Reverse());
            //}

            //if (arcMaxDeg < 90f) {
            builder.Cap (pointsM [n]);
            //}

            return builder.Build ();
        }


        //
        //
        //
        public static Mesh BuildArcSphere(PointListAxis axis, float arcMinDeg, float arcMaxDeg, float radius, int sides)
        {
            Mesh mesh = BuildUnitArcSphere(axis, arcMinDeg, arcMaxDeg, sides);
            MeshAdapter.ScaleVertices (ref mesh, radius);
            return mesh;
        }


        //
        //
        //
        public static Mesh BuildCylinder(PointListAxis axis, float radius, float height, int sides)
        {
            PMesh builder = new PMesh();

            PointList pointsA = PointList.CreatePolygon (axis, radius, sides, -0.5f * height);
            PointList pointsB = pointsA.Translate ( height * Vector (axis) );

            builder.Cap (pointsA.Reverse ());
            builder.Cap (pointsA.Bridge (pointsB, true));
            builder.Cap (pointsB);

            return builder.Build ();
        }

        //
        // Get a vector matching the enum 'PointListAxis'
        //
        private static Vector3 Vector(PointListAxis axis)
        {
            Vector3 result;
            switch (axis) {
            case PointListAxis.XAxis:
                result = new Vector3 (1f, 0, 0);
                break;
            case PointListAxis.YAxis:
                result = new Vector3 (0, 1f, 0);
                break;
            default:
                result = new Vector3 (0, 0, 1f);
                break;
            }
            return result;
        }
    }

}
