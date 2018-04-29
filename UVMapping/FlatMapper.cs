using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Plasticine {

    //
    // Generic Flat UV Mapper
    //
    public class FlatMapper : IUVMapper {

        private Vector3 uVec = Vector3.zero;
        private float   uCst = 0f;

        private Vector3 vVec = Vector3.zero;
        private float   vCst = 0f;

        // Init with 1:1 scale between geometry space and uv space, and no rotation
        // Enough to set uVec and vVec, uCst and vCst are left as is
        public void InitFirstSideV(PointList points)
        {
            vVec = (points [1] - points [0]).normalized;
            uVec = Vector3.Cross(points.ComputeNormal(), vVec).normalized;
        }

        // Init with 1:1 scale between geometry space and uv space, and no rotation
        // Enough to set uVec and vVec, uCst and vCst are left as is
        public void InitLastSideU(PointList points)
        {
            uVec = (points [points.Count - 1] - points [0]).normalized;
            vVec = Vector3.Cross (uVec, points.ComputeNormal ()).normalized;
        }

        // Set uCst and vCSt
        //
        public void SetConstraint(Vector3 point, Vector2 uv)
        {
            uCst = uv.x - (uVec.x * point.x + uVec.y * point.y + uVec.z * point.z);
            vCst = uv.y - (vVec.x * point.x + vVec.y * point.y + vVec.z * point.z);
        }

        public Vector2 GetUV(Vector3 point)
        {
            float u = uVec.x * point.x + uVec.y * point.y + uVec.z * point.z + uCst;
            float v = vVec.x * point.x + vVec.y * point.y + vVec.z * point.z + vCst;
            return new Vector2 (u, v);
        }

    }

}
