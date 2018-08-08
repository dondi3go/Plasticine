using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plasticine;

using NUnit.Framework;

namespace Plasticine {

    [TestFixture]
    public class MeshBuilderTest {
        
        [Test]
        public void TestCreate()
        {
            MeshBuilder builder = new MeshBuilder ();
            Assert.IsNotNull (builder);
        }

        [Test]
        public void TestBuildEmpty()
        {
            MeshBuilder builder = new MeshBuilder ();
            Mesh mesh = builder.Build ();

            List<Vector3> vertices = new List<Vector3> ();
            mesh.GetVertices (vertices);
            Assert.AreEqual (0, vertices.Count);

            List<Vector3> normals = new List<Vector3> ();
            mesh.GetNormals (normals);
            Assert.AreEqual (0, normals.Count);

            List<Vector2> uvs = new List<Vector2> ();
            mesh.GetUVs (0, uvs);
            Assert.AreEqual (0, uvs.Count);
        }

        [Test]
        public void TestBuildOneTriangle()
        {
            PointList list = new PointList ();
            list.Add (0f, 0f, 0f);
            list.Add (1f, 0f, 0f);
            list.Add (0f, 1f, 0f);
            MeshBuilder builder = new MeshBuilder ();
            builder.Cap (list);
            Mesh mesh = builder.Build ();

            List<Vector3> vertices = new List<Vector3> ();
            mesh.GetVertices (vertices);
            Assert.AreEqual (3, vertices.Count);

            List<Vector3> normals = new List<Vector3> ();
            mesh.GetNormals (normals);
            Assert.AreEqual (3, normals.Count); // as many normals as vertices

            List<Vector2> uvs = new List<Vector2> ();
            mesh.GetUVs (0, uvs);
            Assert.AreEqual (3, uvs.Count); // as many uvs as vertices
        }

    }
}
