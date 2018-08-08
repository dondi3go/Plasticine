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
            Assert.IsNotNull (list);
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

        [Test]
        public void TestCopy()
        {
            PointList list0 = new PointList ();
            list0.Add (1f, 1f, 1f);
            int uid0 = list0.Uid (0);

            PointList list1 = new PointList ();
            list1.Copy (list0, 0);
            Assert.AreEqual(uid0, list1.Uid(0));
        }

        [Test]
        public void TestDuplicate()
        {
            PointList list0 = new PointList ();
            list0.Add (0f, 0f, 0f);
            list0.Add (1f, 1f, 1f);
            list0.Add (2f, 2f, 2f);

            PointList list1 = list0.Duplicate ();
            Assert.AreEqual (3, list1.Count);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual (list0 [i], list1 [i]);
                Assert.AreNotEqual (list0.Uid (i), list1.Uid (i));
            }
        }

        [Test]
        public void TestComputeBarycenter()
        {
            PointList list = new PointList ();
            list.Add (0f, 1f, 0f);
            list.Add (0f,-1f, 0f);
            Assert.AreEqual (Vector3.zero, list.ComputeBarycenter());
        }

        [Test]
        public void TestComputeNormal()
        {
            PointList list = new PointList ();
            list.Add (0f, 0f, 0f);
            list.Add (1f, 0f, 0f);
            Assert.AreEqual (Vector3.zero, list.ComputeNormal());
            list.Add (0f, 1f, 0f);
            Assert.AreEqual (new Vector3(0f, 0f, 1f), list.ComputeNormal());
        }

        [Test]
        public void TestReverse()
        {
            PointList list0 = new PointList ();
            list0.Add (0f, 0f, 0f);
            list0.Add (1f, 1f, 1f);
            list0.Add (2f, 2f, 2f);

            PointList list1 = list0.Reverse ();
            Assert.AreEqual (3, list1.Count);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual (list0.Uid (i), list1.Uid (2-i));
            }
        }
    }
}
