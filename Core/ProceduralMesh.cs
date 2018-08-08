using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Plasticine {

    //
    // Procedural Mesh Generation

    //
    public class ProceduralMesh {

        //
        // Link between a position in space and the triangles that use this position
        // See 'Build()' method
        //
        private class PositionLinks
        {
            public Dictionary<int, int> triangleToVertex = new Dictionary<int, int> ();
        }

        //
        // A Triangle
        //
        private class Triangle
        {
            public int[] pIndex = new int[3]; // indices in m_positions
        }

        // Positions (can be shared)
        private List<Vector3> m_positions = new List<Vector3> ();
        private Dictionary<int, int> m_uidToIndex = new Dictionary<int, int> (); // Important for vertex sharing between triangles

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
        // Add a position from a PointList
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
            t.pIndex [1] = m_uidToIndex [uid1];
            t.pIndex [2] = m_uidToIndex [uid2];
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
        // Compute one normal per triangle
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
        // Create one PositionLink per Position
        //
        private List<PositionLinks> ComputePositionLinks ()
        {
            List<PositionLinks> links = new List<PositionLinks>();
            for (int posIndex = 0; posIndex < m_positions.Count; posIndex++) {
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
        // Create Unity Mesh, smooh normals using smoothAngles
        //
        public Mesh Build( float smoothAngle = 45f )
        {
            // Compute normal for each triangle
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
            #if UNITY_EDITOR
            if(uvs.Count>0) {
                Unwrapping.GenerateSecondaryUVSet (mesh);
            }
            #endif
            return mesh;
        }


        //
        //
        //
        public void Add(ProceduralMesh pmesh)
        {
            // TODO : Reindex every position to avoid issues with uids

            int indexOffset = m_positions.Count;

            // Add positions and uvs
            for(int i = 0; i<pmesh.m_positions.Count; i++)
            {
                m_positions.Add ( pmesh.m_positions [i] );
                m_uvs.Add (pmesh.m_uvs [i]);
            }

            // Add uids : does it matter ?
            Dictionary<int, int>.KeyCollection uids = pmesh.m_uidToIndex.Keys;
            foreach (int uid in uids)
            {
                m_uidToIndex [uid] = pmesh.m_uidToIndex[uid] + indexOffset;
            }

            // Add triangles
            for (int i = 0; i < pmesh.m_triangles.Count; i++) {
                Triangle tOther = pmesh.m_triangles [i];
                Triangle tNew = new Triangle ();
                tNew.pIndex[0] = tOther.pIndex[0] + indexOffset;
                tNew.pIndex[1] = tOther.pIndex[1] + indexOffset;
                tNew.pIndex[2] = tOther.pIndex[2] + indexOffset;
                m_triangles.Add (tNew);
            }
        }
    }

}
