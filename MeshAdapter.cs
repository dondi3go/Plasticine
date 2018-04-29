using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Plasticine {

    //
    // Take existing Mesh and perform an operation on it
    //
    public class MeshAdapter {

        //
        //
        //
        public static void RecomputeUVs(ref Mesh mesh, IUVMapper uvMapper)
        {
            List<Vector3> vertices = new List<Vector3> ();
            List<Vector2> uvs = new List<Vector2> ();

            // Recompute UVs from vertices and UV mapper
            mesh.GetVertices (vertices);
            foreach (Vector3 vertex in vertices) {
                uvs.Add (uvMapper.GetUV (vertex));
            }

            mesh.SetUVs (0, uvs);
            Unwrapping.GenerateSecondaryUVSet (mesh);
        }


        //
        // Apply a scale on Vertices, do not change normals, bounds should be recomputed
        //
        public static void ScaleVertices(ref Mesh mesh, Vector3 scale)
        {
            List<Vector3> oldVertices = new List<Vector3> ();
            List<Vector3> newVertices = new List<Vector3> ();

            mesh.GetVertices (oldVertices);
            foreach (Vector3 vertex in oldVertices) {
                vertex.Scale (scale);
                newVertices.Add(vertex);
            }

            mesh.SetVertices (newVertices);
            mesh.RecalculateBounds ();
        }

        //
        // Apply a uniform scale on Vertices
        //
        public static void ScaleVertices(ref Mesh mesh, float scale)
        {
            ScaleVertices (ref mesh, new Vector3(scale, scale, scale));
        }


        //
        // Rotate Vertices using a Quaternion (use Quaternion.AngleAxis() to build it)
        //
        public static void RotateVertices(ref Mesh mesh, Quaternion q)
        {
            List<Vector3> oldVertices = new List<Vector3> ();
            List<Vector3> newVertices = new List<Vector3> ();

            mesh.GetVertices (oldVertices);
            foreach (Vector3 vertex in oldVertices) {
                newVertices.Add(q * vertex);
            }

            mesh.SetVertices (newVertices);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds ();
        }


        //
        // Rotate Vertices (angle in degrees), normals and bounds should be recomputed
        //
        public static void RotateVertices(ref Mesh mesh, Vector3 axis, float angle)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            RotateVertices (ref mesh, q);
        }

    }

}
