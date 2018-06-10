using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plasticine;

//
// Test AutoUVMapping features, drop a texture on each mesh and check everything is OK
//
public class TestMapping : MonoBehaviour {

    public Material material;

    void Start () {
        Debug.Log ("CreateXAxisExtrudeMapping");
        CreateXAxisExtrudeMapping (0f,-1f);
        Debug.Log ("CreateYAxisExtrudeMapping");
        CreateYAxisExtrudeMapping (0f, 0f);
        Debug.Log ("CreateZAxisExtrudeMapping");
        CreateZAxisExtrudeMapping (0f, 1f);

        /*CreateYZFlatMapping (-1f,-1f);
        CreateXYFlatMapping (-1f, 1f);
        CreateXZFlatMapping (-1f, 0f);*/
    }

    private GameObject CreateXAxisExtrudeMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "XAxisExtrudeMapping";

        float x_2 = 0.5f;
        float w_2 = 0.3f;

        PointList pointsA = new PointList ();
        pointsA.Add (-x_2, w_2,-w_2);
        pointsA.Add (-x_2, w_2, w_2);
        pointsA.Add (-x_2,-w_2, w_2);
        pointsA.Add (-x_2,-w_2,-w_2);

        PointList pointsB = pointsA.Translate (new Vector3(2f * x_2, 0, 0));
        List<PointList> list = pointsA.BridgeB (pointsB, PointList.BridgeMode.Open);
        AutoMapper.ComputeUVMapper (ref list);
        AutoMapper.Connect (ref list);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (list);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);
        return obj;
    }

    private GameObject CreateYAxisExtrudeMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "YAxisExtrudeMapping";

        float y_2 = 0.5f;
        float w_2 = 0.3f;

        PointList pointsA = new PointList ();
        pointsA.Add (-w_2,-y_2,-w_2);
        pointsA.Add (-w_2,-y_2, w_2);
        pointsA.Add ( w_2,-y_2, w_2);
        pointsA.Add ( w_2,-y_2,-w_2);

        PointList pointsB = pointsA.Translate (new Vector3(0, 2f * y_2, 0));
        List<PointList> list = pointsA.BridgeB (pointsB, PointList.BridgeMode.CloseDuplicate);
        AutoMapper.ComputeUVMapper (ref list);
        AutoMapper.Connect (ref list);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (list);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);
        return obj;
    }

    private GameObject CreateZAxisExtrudeMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "ZAxisExtrudeMapping";

        float z_2 = 0.5f;
        float w_2 = 0.3f;

        PointList pointsA = new PointList ();
        pointsA.Add ( w_2,-w_2,-z_2);
        pointsA.Add ( w_2, w_2,-z_2);
        pointsA.Add (-w_2, w_2,-z_2);
        pointsA.Add (-w_2,-w_2,-z_2);

        PointList pointsB = pointsA.Translate (new Vector3(0, 0, 2f * z_2));
        List<PointList> list = pointsA.BridgeB (pointsB, PointList.BridgeMode.CloseDuplicate);
        AutoMapper.ComputeUVMapper (ref list);
        AutoMapper.Connect (ref list);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (list);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);
        return obj;
    }

    private GameObject CreateXYFlatMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "XYFlatMapping";

        float w_2 = 0.3f;

        PointList points = new PointList ();
        points.Add ( w_2,-w_2, 0);
        points.Add ( w_2, w_2, 0);
        points.Add (-w_2, w_2, 0);
        points.Add (-w_2,-w_2, 0);

        AutoMapper.ComputeUVMapper (ref points);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (points);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);
        return obj;
    }

    private GameObject CreateXZFlatMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "XZFlatMapping";

        float w_2 = 0.3f;

        PointList points = new PointList ();
        points.Add (-w_2, 0, -w_2);
        points.Add (-w_2, 0,  w_2);
        points.Add ( w_2, 0,  w_2);
        points.Add ( w_2, 0, -w_2);

        AutoMapper.ComputeUVMapper (ref points);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (points);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);
        return obj;
    }

    private GameObject CreateYZFlatMapping(float x, float z)
    {
        GameObject obj = CreateChild (x, z);
        obj.name = "YZFlatMapping";

        float w_2 = 0.3f;

        PointList points = new PointList ();
        points.Add ( 0,-w_2, -w_2);
        points.Add ( 0,-w_2,  w_2);
        points.Add ( 0, w_2,  w_2);
        points.Add ( 0, w_2, -w_2);

        AutoMapper.ComputeUVMapper (ref points);

        ProceduralMesh pmesh = new ProceduralMesh ();
        pmesh.Cap (points);
        Mesh mesh = pmesh.Build ();

        NodeBuilder.SetMesh (obj, mesh);
        NodeBuilder.SetMaterial (obj, material);

        return obj;
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
