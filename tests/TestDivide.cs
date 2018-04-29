﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// Put this script on an empty node
//
public class TestDivide : MonoBehaviour {

    void Start () {
        PointList pointsA = PointList.CreateUnitTile ();
        PointList pointsB = pointsA.Translate (0.2f*Vector3.up);
        List<PointList> list = pointsB.Divide (3, 3);

        MeshBuilder builder = new MeshBuilder ();
        builder.Cap(pointsA.Bridge(pointsB, true));

        foreach(PointList points in list) {
            DigHole (points, builder);
        }

        builder.Cap (pointsA.Reverse ());
        Mesh mesh = builder.Bake ();

        NodeBuilder.AddMesh (gameObject, mesh);
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
