using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public class TestRoundBox : MonoBehaviour {

        // Use this for initialization
        void Start () {
            CreateRoundBox (0, 0);
        }

        void CreateRoundBox(float x, float z)
        {
            GameObject obj = CreateChild (x, z);
            obj.name = "RounBox";
            Mesh mesh = RoundBoxBuilder.BuildRoundBox (1f, 0.3f, 0.7f, 0.2f, 2);
            NodeBuilder.SetMesh (obj, mesh);
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
}
