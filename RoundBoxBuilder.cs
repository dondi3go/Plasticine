using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    // In order to build boxes having rounded corners
    //
    public class RoundBoxBuilder {

        public static Mesh BuildRoundBox(float xSize, float ySize, float zSize, float radius, int sides)
        {
            // Bottom
            PointList pointsA = CreateXZRectangle (xSize - 2f*radius, zSize - 2f*radius, AxisSide.Negative, -ySize*0.5f);
            PointList pointsB = CreateXZRoundRectangle(xSize, zSize, radius, sides, AxisSide.Negative, -ySize*0.5f + radius);

            // Top
            PointList pointsC = CreateXZRoundRectangle(xSize, zSize, radius, sides, AxisSide.Negative, ySize*0.5f - radius);
            PointList pointsD = CreateXZRectangle (xSize - 2f*radius, zSize - 2f*radius, AxisSide.Positive, ySize*0.5f);

            // Create Unity Mesh
            PMesh mesh = new PMesh ();
            mesh.Cap (pointsA);
            mesh.Cap (BridgeRectangleToRoundRectangle(pointsA, pointsB));
            mesh.Cap (pointsC.Bridge(pointsB, true));
            mesh.Cap (BridgeRectangleToRoundRectangle(pointsD.Shift(), pointsC.Reverse()));
            mesh.Cap (pointsD);

            return mesh.Build (1f);
        }


        private static List<PointList> BridgeRectangleToRoundRectangle(PointList rect, PointList roundRect)
        {
            int pointsPerCorner = roundRect.Count / 4;

            List<PointList> list = new List<PointList> ();

            // Corner 1
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 0);
                points.Copy (roundRect, i - 1);
                points.Copy (roundRect, i);
                list.Add (points);
            }

            // Corner 2
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 1);
                points.Copy (roundRect, pointsPerCorner + i - 1);
                points.Copy (roundRect, pointsPerCorner + i);
                list.Add (points);
            }

            // Corner 3
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 2);
                points.Copy (roundRect, 2*pointsPerCorner + i - 1);
                points.Copy (roundRect, 2*pointsPerCorner + i);
                list.Add (points);
            }

            // Corner 4
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 3);
                points.Copy (roundRect, 3*pointsPerCorner + i - 1);
                points.Copy (roundRect, 3*pointsPerCorner + i);
                list.Add (points);
            }

            Debug.Log ("modulo : " + (-1 % 4) );

            // Sides
            for (int i = 1; i <= 4; i++) {
                PointList points = new PointList ();
                points.Copy (rect, i % 4);
                points.Copy (rect, (i - 1) % 4);
                points.Copy (roundRect, (i * pointsPerCorner - 1) % roundRect.Count);
                points.Copy (roundRect, (i * pointsPerCorner) % roundRect.Count);

                list.Add (points);
            }

            return list;
        }

        private static PointList CreateXZRoundRectangle(float xSize, float zSize, float radius, int cornerSides, AxisSide axisSide, float axisCoord)
        {
            PointList result = new PointList ();

            float x = xSize * 0.5f;
            float z = zSize * 0.5f;

            float PI_2 = Mathf.PI * 0.5f;
            float indexToAngle = PI_2 / cornerSides;

            if (axisSide == AxisSide.Negative) {
                // Corner 1
                for(int i=0; i<cornerSides+1; i++) {
                    float angle = PI_2 + i * indexToAngle;
                    result.Add ( -x + radius + radius * Mathf.Cos(angle) , axisCoord, z - radius + radius * Mathf.Sin(angle) );
                }

                // Corner 2
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = Mathf.PI + i * indexToAngle;
                    result.Add ( -x + radius + radius * Mathf.Cos(angle) , axisCoord, -z + radius + radius * Mathf.Sin(angle) );
                }

                // Corner 3
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = Mathf.PI + PI_2 + i * indexToAngle;
                    result.Add ( x - radius + radius * Mathf.Cos(angle) , axisCoord, -z + radius + radius * Mathf.Sin(angle) );
                }

                // Corner 4
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = i * indexToAngle;
                    result.Add ( x - radius + radius * Mathf.Cos(angle) , axisCoord, z - radius + radius * Mathf.Sin(angle) );
                }

            } else {
                result.Add (-x, 0, z);
                result.Add (x, 0, z);
                result.Add (x, 0, -z);
                result.Add (-x, 0, -z);
            }

            return result;
        }

        private static PointList CreateXZRectangle(float xSize, float zSize, AxisSide axisSide, float axisCoord)
        {
            PointList result = new PointList ();

            float x = xSize * 0.5f;
            float z = zSize * 0.5f;

            if (axisSide == AxisSide.Negative) {
                result.Add (-x, axisCoord, z);
                result.Add (-x, axisCoord, -z);
                result.Add (x, axisCoord, -z);
                result.Add (x, axisCoord, z);
            } else {
                result.Add (-x, axisCoord, z);
                result.Add (x, axisCoord, z);
                result.Add (x, axisCoord, -z);
                result.Add (-x, axisCoord, -z);
            }

            return result;
        }
    }
}
