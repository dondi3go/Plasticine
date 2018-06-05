using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Plasticine {

    public class MeshBuilder {

        private List<Vector3> vertices = new List<Vector3> ();
        private List<Vector2> uvs = new List<Vector2> ();
        private List<int> triangles = new List<int> ();

        //
        //
        //
        public void Cap(PointList points) {

            int offset = vertices.Count;

            foreach(Vector3 point in points) {

                vertices.Add (point);
                uvs.Add ( points.UVMapper.GetUV(point) );
            }

            if (points.Count == 3) {
                triangles.Add (offset);
                triangles.Add (offset+1);
                triangles.Add (offset+2);
            }
            else if (points.Count == 4) {
                triangles.Add (offset);
                triangles.Add (offset+1);
                triangles.Add (offset+2);

                triangles.Add (offset);
                triangles.Add (offset+2);
                triangles.Add (offset+3);
            } else {
                for(int i=0; i<points.Count-2; i++) {
                    triangles.Add (offset);
                    triangles.Add (offset+i+1);
                    triangles.Add (offset+i+2);
                }
            }
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
        public Mesh Bake() {

            Mesh mesh = new Mesh ();

            mesh.SetVertices(vertices);
            mesh.SetUVs (0, uvs);
            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateNormals();
            mesh.RecalculateBounds ();
            #if UNITY_EDITOR
            Unwrapping.GenerateSecondaryUVSet (mesh);
            #endif
            return mesh;
        }
    }

}
