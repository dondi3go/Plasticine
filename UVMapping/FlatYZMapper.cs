using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public class FlatYZMapper : IUVMapper {

        private float m_uY = 0f;
        private float m_uZ = 1f;

        private float m_vY = 1f;
        private float m_vZ = 0f;

        private float m_du = 0.5f;
        private float m_dv = 0f;

        public Vector2 GetUV(Vector3 point)
        {
            float u = m_uY * point.y + m_uZ * point.z + m_du;
            float v = m_vY * point.y + m_vZ * point.z + m_dv;
            return new Vector2 (u, v);
        }

        public void SetRotation(float angleRad)
        {
            float cos = Mathf.Cos (angleRad);
            float sin = Mathf.Sin (angleRad);

            m_uY = sin;
            m_uZ = cos;

            m_vY = -cos;
            m_vZ = -sin;
        }

        public void SetConstraint (Vector3 point, Vector2 uv)
        {
            m_du = uv.x - (m_uY * point.y + m_uZ * point.z);
            m_dv = uv.y - (m_vY * point.y + m_vZ * point.z);
        }

    }

}