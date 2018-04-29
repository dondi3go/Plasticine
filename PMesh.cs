using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Plasticine {

//
// Link between a position in space and the triangles that use this position
//
public class PositionLinks
{
    public Dictionary<int, int> triangleToVertex = new Dictionary<int, int> ();
}


//
// A Triangle
//
public class Triangle
{
    public int[] pIndex = new int[3]; // indices in m_positions
    public int[] uvIndex = new int[3]; // id in uvs
}


//
// Procedural Mesh Generation
// Ranem it into SHAPE
//
public class PMesh {

    // Positions (can be shared)
    private List<Vector3> m_positions = new List<Vector3> ();
    private Dictionary<int, int> m_uidToIndex = new Dictionary<int, int> (); // Important for PointList usage

    // UVs
    private List<Vector2> m_uvs = new List<Vector2> ();

    // Triangles
    private List<Triangle> m_triangles = new List<Triangle> ();

    //
    //
    //
    public void Cap(PointList points) {
        
        // Add positions
        for(int i=0; i<points.Count; i++) {
            AddPosition (points, i);
        }

        // Add triangles
        for (int i = 0; i < points.Count - 2; i++) {
            AddTriangle (points.Uid (0), points.Uid (i + 1), points.Uid (i + 2));
        }
    }

    //
    // Warning : here we assume uvs and positions share the same index ... NOT SURE AT ALL !
    //
    private void AddPosition(PointList points, int i)
    {
        int uid = points.Uid (i);
        if (!m_uidToIndex.ContainsKey (uid))
        {
            Vector3 point = points [i];
            int index = m_positions.Count;

            m_positions.Add ( point );
            m_uvs.Add ( points.UVMapper.GetUV(point) );
            m_uidToIndex [uid] = index;
        }
    }

    private void AddTriangle(int uid0, int uid1, int uid2)
    {
        Triangle t = new Triangle ();

        t.pIndex [0] = m_uidToIndex [uid0];
        t.uvIndex [0] = m_uidToIndex [uid0];

        t.pIndex [1] = m_uidToIndex [uid1];
        t.uvIndex [1] = m_uidToIndex [uid1];

        t.pIndex [2] = m_uidToIndex [uid2];
        t.uvIndex [2] = m_uidToIndex [uid2];

        m_triangles.Add (t);
    }

    //
    //
    //
    public void Cap(List<PointList> list) {
        
        foreach (PointList points in list) {
            
            Cap (points);
        }
    }

    //
    //
    //
    private List<Vector3> ComputeFlatNormals()
    {
        List<Vector3> flatNormals = new List<Vector3> ();
        for (int triIndex = 0; triIndex < m_triangles.Count; triIndex++) {
            Triangle tri = m_triangles [triIndex];
            Vector3 v0 = m_positions [tri.pIndex [0]];
            Vector3 v1 = m_positions [tri.pIndex [1]];
            Vector3 v2 = m_positions [tri.pIndex [2]];
            flatNormals.Add( Vector3.Cross (v1-v0, v2-v0).normalized );
        }
        return flatNormals;
    }

    //
    //
    //
    private List<PositionLinks> ComputePositionLinks ()
    {
        List<PositionLinks> links = new List<PositionLinks>();
        for (int posIndex = 0; posIndex < m_triangles.Count; posIndex++) {
            links.Add(new PositionLinks());
        }
        for (int triIndex = 0; triIndex < m_triangles.Count; triIndex++) {
            Triangle tri = m_triangles [triIndex];
            links [tri.pIndex [0]].triangleToVertex.Add(triIndex, -1);
            links [tri.pIndex [1]].triangleToVertex.Add(triIndex, -1);
            links [tri.pIndex [2]].triangleToVertex.Add(triIndex, -1);
        }
        return links;
    }

    //
    //
    //
    public Mesh Build( float smoothAngle = 45f )
    {
        List<Vector3> flatNormals = ComputeFlatNormals ();

        // Compute PositionLink, one per position, init with -1
        List<PositionLinks> links = ComputePositionLinks();

        // Data for Mesh creation
        List<Vector3> vertices = new List<Vector3> ();
        List<Vector2> uvs = new List<Vector2> ();
        List<int> triangles = new List<int> ();

        for (int triIndex = 0; triIndex < m_triangles.Count; triIndex++) {
            Triangle tri = m_triangles[triIndex];
            for (int i = 0; i < 3; i++) {
                int posIndex = tri.pIndex [i];
                int vtxIndex = links [posIndex].triangleToVertex [triIndex];
                if (vtxIndex != -1) {
                    // Vertex already known
                    triangles.Add (vtxIndex);
                } else {
                    // Create new Vertex
                    vtxIndex = vertices.Count;
                    vertices.Add (m_positions[posIndex]);
                    uvs.Add (m_uvs [posIndex]);
                    // Use new vertex
                    triangles.Add (vtxIndex);
                    // Make all triangles whose angle is under limitAngle also use new vertex
                    int keysCount = links [posIndex].triangleToVertex.Keys.Count;
                    int[] keys = new int[keysCount];
                    links [posIndex].triangleToVertex.Keys.CopyTo (keys, 0);
                    for (int j = 0; j < keysCount; j++) {
                        if(Vector3.Angle( flatNormals[triIndex], flatNormals[keys[j]] ) < smoothAngle) {
                            links [posIndex].triangleToVertex [keys[j]] = vtxIndex;
                        }
                    }
                }
            }
        }

        // Create Mesh
        Mesh mesh = new Mesh ();
        mesh.SetVertices(vertices);
        mesh.SetUVs (0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds ();
        Unwrapping.GenerateSecondaryUVSet (mesh);
        return mesh;
    }

}

}
