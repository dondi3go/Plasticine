using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

//
// 
//
public class NodeBuilder {

    //
    // Add a Mesh to a GameObject
    //
    public static void SetMesh(GameObject obj, Mesh mesh)
    {
        MeshFilter filter = GetMeshFilter(obj);
        GetMeshRenderer (obj); // Set a default Material
        filter.sharedMesh = mesh;
    }

    //
    //
    //
    public static Mesh GetMesh(GameObject obj)
    {
        MeshFilter filter = obj.GetComponent<MeshFilter>();
        if (filter == null) {
            return null;
        }
        return filter.sharedMesh;
    }

    //
    // Add a Material to a GameObject (replace default one)
    //
    public static void SetMaterial(GameObject obj, Material mat)
    {
        MeshRenderer renderer = GetMeshRenderer(obj);
        renderer.sharedMaterial = mat;
    }

    //
    // 
    //
    public static MeshRenderer GetMeshRenderer(GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer> ();
        if (renderer == null) {
            // Add missing MeshRenderer
            renderer = obj.AddComponent<MeshRenderer>();
            // Add default material if none
            if (renderer.sharedMaterial == null) {
                renderer.sharedMaterial = new Material (Shader.Find ("Standard"));
                renderer.sharedMaterial.name = "default";
            }
        }
        return renderer;
    }

    //
    //
    //
    public static MeshFilter GetMeshFilter(GameObject obj)
    {
        MeshFilter filter = obj.GetComponent<MeshFilter>();
        if (filter == null) {
            // Add missing MeshFilter
            filter = obj.AddComponent<MeshFilter>();
        }
        return filter;
    }

    //
    //
    //
    public static void DestroyChildren(GameObject obj)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in obj.transform) 
            children.Add(child.gameObject);
        foreach(GameObject child in children)
            GameObject.DestroyImmediate(child);
    }

}

}
