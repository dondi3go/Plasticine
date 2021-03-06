﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// Put this script on an empty node
//
public class TestPrimitives : MonoBehaviour {

    void Start () {

        float tStart = Time.realtimeSinceStartup;
        CreateCube (0f, -2f);

        CreateXSphere (-2f, 0f);
        CreateYSphere (0f, 0f);
        CreateZSphere (2f, 0f);

        CreateXCylinder (-2f, 2f);
        CreateYCylinder (0f, 2f);
        CreateZCylinder (2f, 2f);

        float tSpan = Time.realtimeSinceStartup - tStart;
        Debug.Log ("Creation duration = " + tSpan.ToString("0.000") + " seconds");
    }

    void CreateCube(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "Cube";
        Mesh mesh = PrimitiveBuilder.BuildCube (1f);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateXSphere(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "XSphere";
        Mesh mesh = PrimitiveBuilder.BuildSphere (Axis.XAxis, 0.5f, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateYSphere(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "YSphere";
        Mesh mesh = PrimitiveBuilder.BuildSphere (Axis.YAxis, 0.5f, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateZSphere(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "ZSphere";
        Mesh mesh = PrimitiveBuilder.BuildSphere (Axis.ZAxis, 0.5f, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateXCylinder(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "XCylinder";
        Mesh mesh = PrimitiveBuilder.BuildCylinder (Axis.XAxis, 0.5f, 1, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateYCylinder(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "YCylinder";
        Mesh mesh = PrimitiveBuilder.BuildCylinder (Axis.YAxis, 0.5f, 1, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    //
    //
    //
    void CreateZCylinder(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "ZCylinder";
        Mesh mesh = PrimitiveBuilder.BuildCylinder (Axis.ZAxis, 0.5f, 1, 24);
        NodeBuilder.SetMesh (obj, mesh);
    }

    // -------------------------------------

    //
    // Create a new node under current node
    // 
    private GameObject CreateChild(float x, float z)
    {
        GameObject obj = new GameObject ();
        obj.transform.SetParent (transform);
        obj.transform.localPosition = new Vector3 (x, 0, z);
        return obj;
    }

}

}
