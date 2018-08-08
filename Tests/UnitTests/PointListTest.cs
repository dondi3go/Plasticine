using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plasticine;

using NUnit.Framework;

namespace Plasticine {

    [TestFixture]
    public class PointListTest {

        [Test]
        public void TestCreate()
        {
            PointList list = new PointList ();
        }

        [Test]
        public void TestAdd()
        {
            PointList list = new PointList ();
            list.Add (new Vector3 (1f, 1f, 1f));
            Assert.AreEqual (1, list.Count);
            list.Add (2f, 2f, 2f);
            Assert.AreEqual (2, list.Count);
        }

        [Test]
        public void TestClear()
        {
            PointList list = new PointList ();
            list.Add (new Vector3 (1f, 1f, 1f));
            Assert.AreEqual (1, list.Count);
            list.Clear ();
            Assert.AreEqual (0, list.Count);
        }
    }
}
