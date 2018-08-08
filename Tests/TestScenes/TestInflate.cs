using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// Put this script on an empty node
//
public class TestInflate : MonoBehaviour {


    void Start () {

        MeshBuilder builder = new MeshBuilder ();

        PointList pointsZ = PrimitiveBuilder.CreateUnitTile ();
        PointList pointsA = pointsZ.Translate (new Vector3 (0f, -0.5f, 0f));
        PointList pointsB = pointsA.Translate (Vector3.up);
        List<PointList> list = pointsA.Bridge(pointsB, true);
        list.Add (pointsA.Reverse ());
        list.Add (pointsB);
        foreach(PointList points in list) {
            SphereSide (points, builder, 5);
        }
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (gameObject, mesh);
    }

    void SphereSide(PointList points, MeshBuilder builder, int depth) {

        List<PointList> list = points.Divide (4, 4);
        foreach(PointList p in list) {
            PointList newP = p.Inflate (Vector3.zero, 1f, 1f);
            Extend(newP, builder);
        }
    }

    void Extend(PointList points, MeshBuilder builder) {
        Vector3 n = points.ComputeNormal ();
        PointList pointsB = points.Translate (0.4f*n.normalized);
        builder.Cap(points.Bridge(pointsB, true));
        DigHole (pointsB, builder);
    }

    void DigHole(PointList points, MeshBuilder builder) {
        Vector3 n = points.ComputeNormal ();
        PointList pointsC = points.Scale (0.5f);
        PointList pointsD = pointsC.Translate (-0.1f*n.normalized);
        builder.Cap(points.Bridge(pointsC, true));
        builder.Cap(pointsC.Bridge(pointsD, true));
        builder.Cap (pointsD);
    }

}

}
