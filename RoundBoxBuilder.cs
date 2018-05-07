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
            // Bottom rectangle
            PointList pointsB = CreateXZRectangle (xSize - 2f*radius, zSize - 2f*radius, AxisSide.Positive, -ySize*0.5f);

            // Bottom sides
            List<PointList> steps = new List<PointList>();
            for (int i = 1; i < sides + 1; i++) {
                float a = - Mathf.PI / 2f  + i * Mathf.PI / (2f * sides);
                float r = radius * Mathf.Cos (a);
                steps.Add( CreateXZRoundRectangle(xSize - 2f*(radius - r), zSize - 2f*(radius - r), r, sides, AxisSide.Positive, -ySize*0.5f + radius + radius * Mathf.Sin(a)) );
            }

            // Top sides
            for (int i = 0; i < sides ; i++) {
                float a = i * Mathf.PI / (2f * sides);
                float r = radius * Mathf.Cos (a);
                steps.Add( CreateXZRoundRectangle(xSize - 2f*(radius - r), zSize - 2f*(radius - r), r, sides, AxisSide.Positive, ySize*0.5f - radius + radius * Mathf.Sin(a)) );
            }

            // Top rectangle
            PointList pointsT = CreateXZRectangle (xSize - 2f*radius, zSize - 2f*radius, AxisSide.Positive, ySize*0.5f);

            // Polygons
            List<PointList> list = new List<PointList>();
            for (int i = 0; i < steps.Count - 1; i++) {
                list.AddRange( steps [i].Bridge (steps [i + 1], true));
            }
            list.AddRange(BridgeRectangleToRoundRectangle (pointsB, steps [0])); // bottom 
            list.AddRange(BridgeRoundRectangleToRectangle (steps [steps.Count - 1], pointsT)); // top

            // Create Unity Mesh
            ProceduralMesh mesh = new ProceduralMesh ();
            mesh.Cap (pointsB.Reverse());
            mesh.Cap (list);
            mesh.Cap (pointsT);
            return mesh.Build ();
        }

        private static List<PointList> BridgeRoundRectangleToRectangle(PointList roundRect, PointList rect)
        {
            return BridgeRectangleToRoundRectangle (rect.Reverse (), roundRect.Reverse ());
        }

        private static List<PointList> BridgeRectangleToRoundRectangle(PointList rect, PointList roundRect)
        {
            int pointsPerCorner = roundRect.Count / 4;

            List<PointList> list = new List<PointList> ();

            // Corner 1
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 0);
                points.Copy (roundRect, i);
                points.Copy (roundRect, i - 1);
                list.Add (points);
            }

            // Corner 2
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 1);
                points.Copy (roundRect, pointsPerCorner + i);
                points.Copy (roundRect, pointsPerCorner + i - 1);
                list.Add (points);
            }

            // Corner 3
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 2);
                points.Copy (roundRect, 2*pointsPerCorner + i);
                points.Copy (roundRect, 2*pointsPerCorner + i - 1);
                list.Add (points);
            }

            // Corner 4
            for(int i=1; i<pointsPerCorner; i++) {
                PointList points = new PointList ();
                points.Copy (rect, 3);
                points.Copy (roundRect, 3*pointsPerCorner + i);
                points.Copy (roundRect, 3*pointsPerCorner + i - 1);
                list.Add (points);
            }

            // Sides
            for (int i = 1; i <= 4; i++) {
                PointList points = new PointList ();
                points.Copy (rect, i % 4);
                points.Copy (roundRect, (i * pointsPerCorner) % roundRect.Count);
                points.Copy (roundRect, (i * pointsPerCorner - 1) % roundRect.Count);
                points.Copy (rect, (i - 1) % 4);

                list.Add (points);
            }

            return list;
        }

        private static PointList CreateXZRoundRectangle(float xSize, float zSize, float radius, int cornerSides, AxisSide axisSide, float axisCoord)
        {
            PointList result = new PointList ();

            float x = xSize * 0.5f - radius;
            float z = zSize * 0.5f - radius;

            float PI_2 = Mathf.PI * 0.5f;
            float indexToAngle = PI_2 / cornerSides;

            if (axisSide == AxisSide.Negative) {
                
                // Corner 1
                for(int i=0; i<cornerSides+1; i++) {
                    float angle = PI_2 + i * indexToAngle;
                    result.Add ( -x + radius * Mathf.Cos(angle) , axisCoord, z + radius * Mathf.Sin(angle) );
                }

                // Corner 2
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = Mathf.PI + i * indexToAngle;
                    result.Add ( -x + radius * Mathf.Cos(angle) , axisCoord, -z + radius * Mathf.Sin(angle) );
                }

                // Corner 3
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = - PI_2 + i * indexToAngle;
                    result.Add ( x + radius * Mathf.Cos(angle) , axisCoord, -z + radius * Mathf.Sin(angle) );
                }

                // Corner 4
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = i * indexToAngle;
                    result.Add ( x + radius * Mathf.Cos(angle) , axisCoord, z + radius * Mathf.Sin(angle) );
                }

            } else {

                // Corner 1
                for(int i=0; i<cornerSides+1; i++) {
                    float angle =  Mathf.PI - i * indexToAngle;
                    result.Add ( -x + radius * Mathf.Cos(angle) , axisCoord, z + radius * Mathf.Sin(angle) );
                }

                // Corner 4
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = PI_2 - i * indexToAngle;
                    result.Add ( x + radius * Mathf.Cos(angle) , axisCoord, z + radius * Mathf.Sin(angle) );
                }

                // Corner 3
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = - i * indexToAngle;
                    result.Add ( x + radius * Mathf.Cos(angle) , axisCoord, -z + radius * Mathf.Sin(angle) );
                }

                // Corner 2
                for (int i = 0; i < cornerSides + 1; i++) {
                    float angle = - PI_2 - i * indexToAngle;
                    result.Add ( -x + radius * Mathf.Cos(angle) , axisCoord, -z + radius * Mathf.Sin(angle) );
                }

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
