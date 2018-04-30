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
            PointList pointsA = CreateXZRectangle (xSize - 2f*radius, zSize - 2f*radius, AxisSide.Negative);

            // Create Unity Mesh
            PMesh mesh = new PMesh ();
            mesh.Cap (pointsA);
            return mesh.Build ();
        }


        private static List<PointList> BridgeRectangleToRoundRectangle(PointList rect, PointList roundRect)
        {
            List<PointList> list = new List<PointList> ();
            return list;
        }

        private static PointList CreateXZRoundRectangle(float xSize, float zSize, float radius, int sides)
        {
            PointList result = new PointList ();

            return result;
        }

        private static PointList CreateXZRectangle(float xSize, float zSize, AxisSide axisSide)
        {
            PointList result = new PointList ();

            float x = xSize * 0.5f;
            float z = zSize * 0.5f;

            if (axisSide == AxisSide.Negative) {
                result.Add (-x, 0, z);
                result.Add (-x, 0, -z);
                result.Add (x, 0, -z);
                result.Add (x, 0, z);
            } else {
                result.Add (-x, 0, z);
                result.Add (x, 0, z);
                result.Add (x, 0, -z);
                result.Add (-x, 0, -z);
            }

            return result;
        }
    }
}
