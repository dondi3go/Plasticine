using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// Put this script on an empty node
//
public class TestBranch : MonoBehaviour {


    void Start () {

        MeshBuilder builder = new MeshBuilder ();

        // Create a cube
        PointList pointsA = PointList.CreateUnitTile ();
        PointList pointsB = pointsA.Translate (Vector3.up);
        List<PointList> list = pointsA.Bridge(pointsB, true);
        list.Add (pointsA.Reverse ());
        list.Add (pointsB);

        // Call Branch() on each face
        foreach(PointList points in list) {
            Branch (points, builder, 8);
        }
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (gameObject, mesh);
    }

    //
    // Warning : reccursive method
    // If depth is too high, it will not work since a Mesh is limited to 65535 vertices
    //
    void Branch(PointList points, MeshBuilder builder, int depth) {

        if (depth == 0) {
            builder.Cap(points);
            return;
        }

        List<PointList> list = points.Divide (2, 1);

        Vector3 n0 = list [0].ComputeNormal ();
        Vector3 t0 = list [0] [1] - list [0] [0];
        Vector3 e0 = 0.5f * n0.normalized - 0.15f * t0.normalized;
        PointList points0 = list [0].Extrude (list [0].ComputeBarycenter(), e0, e0).Scale(0.8f);
        builder.Cap (list [0].Bridge(points0, true));
        Branch (points0.Shift(), builder, depth - 1);

        Vector3 n1 = list [1].ComputeNormal();
        Vector3 t1 = list [1] [1] - list [1] [0];
        Vector3 e1 = 0.5f * n1.normalized + 0.15f * t1.normalized;
        PointList points1 = list [1].Extrude (list [1].ComputeBarycenter(), e1, e1).Scale(0.8f);
        builder.Cap (list [1].Bridge(points1, true));
        Branch (points1.Shift(), builder, depth - 1);
    }

}

}
