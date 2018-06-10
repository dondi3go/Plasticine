using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plasticine;

using NUnit.Framework;

namespace Plasticine {

    [TestFixture]
    public class ProceduralMeshTest {

        public ProceduralMeshTest() {
        }

        [Test]
        public void Create()
        {
            //ProceduralMesh pmesh = new ProceduralMesh ();
            int i=2;
            Assert.AreEqual (i, 1);
        }

    }
}
