using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// Put this script on an empty node
//
public class TestBasicShapes : MonoBehaviour {

    void Start () {

        // 3
        CreateTriangle (-2f, 0);
        CreateTriangleUp(-2f, 2f);
        CreateTriangleHop(-2f, 4f);

        // 4
        CreateUnitTile (0, 0);
        CreateUnitTileUp (0, 2f);
        CreateUnitTileHop (0, 4f);

        // 5
        CreatePentagon (2f, 0);
        CreatePentagonUp (2f, 2f);
        CreatePentagonHop (2f, 4f);
    }

    //
    //
    //
    private GameObject CreateUnitTile(float x, float z)
    {
        GameObject obj = CreateChild (x, z);

        PointList points = PointList.CreateUnitTile ();
        MeshBuilder builder = new MeshBuilder ();
        builder.Cap (points);
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreateUnitTileUp(float x, float z)
    {
        GameObject obj = CreateChild (x, z);

        PointList pointsA = PointList.CreateUnitTile ();
        PointList pointsB = pointsA.Translate (Vector3.up);

        List<PointList> list = pointsA.Bridge (pointsB, true);

        MeshBuilder builder = new MeshBuilder ();
        builder.Cap (list);
        builder.Cap (pointsB);
        builder.Cap (pointsA.Reverse());
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreateUnitTileHop(float x, float z)
    {
        GameObject obj = CreateChild (x, z);

        MeshBuilder builder = new MeshBuilder ();
        PointList pointsA = PointList.CreateUnitTile ();
        PointList pointsB = pointsA.Translate (Vector3.up);
        List<PointList> list = pointsA.Bridge(pointsB, true);
        list.Add (pointsA.Reverse ());
        list.Add (pointsB);
        foreach(PointList points in list) {
            Extend (points, builder);
        }
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    // -------------------------------------

    private GameObject CreateTriangle(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        builder.Cap (PointList.CreateUnitPolygon (3));
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreateTriangleUp(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        PointList pointsA = PointList.CreateUnitPolygon (3);
        PointList pointsB = pointsA.Translate(new Vector3(0f, 1f, 0f));
        builder.Cap ( pointsA.Bridge(pointsB, true) );
        builder.Cap ( pointsB );
        builder.Cap ( pointsA.Reverse() );
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreateTriangleHop(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        PointList pointsA = PointList.CreateUnitPolygon (3);
        PointList pointsB = pointsA.Translate(new Vector3(0f, 1f, 0f));
        List<PointList> list = pointsA.Bridge(pointsB, true);
        list.Add( pointsB );
        list.Add ( pointsA.Reverse() );
        foreach(PointList points in list) {
            Extend (points, builder);
        }
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    // -------------------------------------

    private GameObject CreatePentagon(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        builder.Cap (PointList.CreateUnitPolygon (5));
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreatePentagonUp(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        PointList pointsA = PointList.CreateUnitPolygon (5);
        PointList pointsB = pointsA.Translate(new Vector3(0f, 1f, 0f));
        builder.Cap ( pointsA.Bridge(pointsB, true) );
        builder.Cap ( pointsB );
        builder.Cap ( pointsA.Reverse() );
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    private GameObject CreatePentagonHop(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        
        MeshBuilder builder = new MeshBuilder ();
        PointList pointsA = PointList.CreateUnitPolygon (5);
        PointList pointsB = pointsA.Translate(new Vector3(0f, 1f, 0f));
        List<PointList> list = pointsA.Bridge(pointsB, true);
        list.Add( pointsB );
        list.Add ( pointsA.Reverse() );
        foreach(PointList points in list) {
            Extend (points, builder);
        }
        Mesh mesh = builder.Bake ();

        NodeBuilder.SetMesh (obj, mesh);
        return obj;
    }

    // -------------------------------------

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
