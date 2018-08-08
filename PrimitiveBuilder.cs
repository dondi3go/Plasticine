using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    //
    //
    public class PrimitiveBuilder {

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
        public static PointList CreatePolygon(Axis axis, float radius, int sides, float axisCoord = 0f)
        {
            PointList result = new PointList ();
            float indexToAngle = Mathf.PI*2f/(float)sides;
            switch(axis)
            {
            case Axis.XAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (axisCoord, radius*Mathf.Cos(angle), radius*Mathf.Sin(angle));
                }
                break;

            case Axis.YAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (radius*Mathf.Sin(angle), axisCoord, radius*Mathf.Cos(angle));
                }
                break;

            case Axis.ZAxis:
                for (int i = 0; i < sides; i++) {
                    float angle = i * indexToAngle;
                    result.Add (radius*Mathf.Cos(angle), radius*Mathf.Sin(angle), axisCoord);
                }
                break;
            }
            return result;
        }


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
            MeshBuilder builder = new MeshBuilder();

            PointList pointsA = new PointList ();
            pointsA.Add (xMin, yMin, zMin);
            pointsA.Add (xMin, yMin, zMax);
            pointsA.Add (xMax, yMin, zMax);
            pointsA.Add (xMax, yMin, zMin);

            PointList pointsB = pointsA.Translate ( new Vector3(0f, yMax - yMin, 0f) );

            builder.Cap (pointsA.Reverse());
            builder.Cap (pointsA.Bridge (pointsB, PointList.BridgeMode.CloseReuse));
            builder.Cap (pointsB);

            return builder.Build();
        }


        //
        //
        //
        public static Mesh BuildSphere(Axis axis, float radius, int sides)
        {
            if (radius > 0.02f) {
                
                // Works great up to radius = 0.02, under this limit normal issues appear

                MeshBuilder builder = new MeshBuilder ();

                PointList points = CreatePolygon (axis, radius, sides);

                int n = sides / 4;

                PointList[] pointsM = new PointList[n + 1];
                pointsM [0] = points;

                Vector3 v = Vector (axis);

                float indexToRadAngle = Mathf.PI * 0.5f / n;

                // First Hemisphere
                for (int i = 1; i <= n; i++) {
                    float angle = i * indexToRadAngle;
                    pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                    builder.Cap (pointsM [i - 1].Bridge (pointsM [i], PointList.BridgeMode.CloseReuse));
                }

                // Second Hemisphere
                for (int i = 1; i <= n; i++) {
                    float angle = -i * indexToRadAngle;
                    pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                    builder.Cap (pointsM [i].Bridge (pointsM [i - 1], PointList.BridgeMode.CloseReuse));
                }

                return builder.Build ();
            
            } else {
                
                Mesh mesh = BuildSphere (axis, 1.0f, sides);
                MeshAdapter.ScaleVertices (ref mesh, radius);
                return mesh;
            }
        }


        //
        //
        //
        public static Mesh BuildHemisphere(Axis axis, AxisSide axisSide, float radius, int sides)
        {
            if (radius > 0.02f) {

                // Works great up to radius = 0.02, under this limit normal issues appear

                MeshBuilder builder = new MeshBuilder ();

                PointList points = CreatePolygon (axis, radius, sides);

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
                        builder.Cap (pointsM [i - 1].Bridge (pointsM [i], PointList.BridgeMode.CloseReuse));
                    }
                } else {

                    // Second Hemisphere
                    for (int i = 1; i <= n; i++) {
                        float angle = -i * indexToRadAngle;
                        pointsM [i] = points.Scale (Mathf.Cos (angle)).Translate (radius * Mathf.Sin (angle) * v);
                        builder.Cap (pointsM [i].Bridge (pointsM [i - 1], PointList.BridgeMode.CloseReuse));
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
        public static Mesh BuildUnitArcSphere(Axis axis, float arcMinDeg, float arcMaxDeg, int sides)
        {
            MeshBuilder builder = new MeshBuilder ();

            int n = Mathf.FloorToInt((arcMaxDeg - arcMinDeg) * sides / 360f);

            PointList[] pointsM = new PointList[n + 1];

            for (int i = 0; i <= n; i++) {
                float angle = Mathf.Deg2Rad * (arcMinDeg + i * (arcMaxDeg - arcMinDeg) / n);
                pointsM[i] = CreatePolygon (axis, Mathf.Cos (angle), sides, Mathf.Sin (angle));
                if (i > 0) {
                    builder.Cap (pointsM [i-1].Bridge (pointsM [i], PointList.BridgeMode.CloseReuse));
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
        public static Mesh BuildArcSphere(Axis axis, float arcMinDeg, float arcMaxDeg, float radius, int sides)
        {
            Mesh mesh = BuildUnitArcSphere(axis, arcMinDeg, arcMaxDeg, sides);
            MeshAdapter.ScaleVertices (ref mesh, radius);
            return mesh;
        }


        //
        //
        //
        public static Mesh BuildCylinder(Axis axis, float radius, float height, int sides)
        {
            MeshBuilder builder = new MeshBuilder();

            PointList pointsA = CreatePolygon (axis, radius, sides, -0.5f * height);
            PointList pointsB = pointsA.Translate ( height * Vector (axis) );

            builder.Cap (pointsA.Reverse ());
            builder.Cap (pointsA.Bridge (pointsB, PointList.BridgeMode.CloseReuse));
            builder.Cap (pointsB);

            return builder.Build ();
        }

        //
        // Get a vector matching the enum 'PointListAxis'
        //
        private static Vector3 Vector(Axis axis)
        {
            Vector3 result;
            switch (axis) {
            case Axis.XAxis:
                result = new Vector3 (1f, 0, 0);
                break;
            case Axis.YAxis:
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
