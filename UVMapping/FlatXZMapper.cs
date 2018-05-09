using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    //
    // We always have UV(0, 0, 0) = (0.5, O.5)
    //
    public class FlatXZMapper : IUVMapper {

        private float m_uX = 0f;
        private float m_uZ = 1f;

        private float m_vX =-1f;
        private float m_vZ = 0f;

        private float m_du = 0.5f;
        private float m_dv = 0.5f;

        public Vector2 GetUV(Vector3 point)
        {
            float u = m_uX * point.x + m_uZ * point.z + m_du;
            float v = m_vX * point.x + m_vZ * point.z + m_dv;
            return new Vector2 (u, v);
        }

        public void SetRotation (float angleRad) {

            float cos = Mathf.Cos (angleRad);
            float sin = Mathf.Sin (angleRad);

            m_uX = sin;
            m_uZ = cos;

            m_vX = -cos;
            m_vZ = -sin;
        }

        public void SetConstraint (Vector3 point, Vector2 uv)
        {
            m_du = uv.x - (m_uX * point.x + m_uZ * point.z);
            m_dv = uv.y - (m_vX * point.x + m_vZ * point.z);
        }

    }

}
