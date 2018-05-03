
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    // TODO : have a generic version : Translation / Rotation / Scale
    //
    public class FlatXYMapper : IUVMapper {

        private float m_scale = 1f;

        private float m_uX = 1f;
        private float m_uY = 0f;

        private float m_vX = 0f;
        private float m_vY = 1f;

        private float m_du = 0.5f;
        private float m_dv = 0f;


        public Vector2 GetUV(Vector3 point)
        {
            float u = m_scale * (m_uX * point.x + m_uY * point.y) + m_du;
            float v = m_scale * (m_vX * point.x + m_vY * point.y) + m_dv;
            return new Vector2 (u, v);
        }

        public void SetScale(float scale)
        {
            m_scale = scale;
        }

        public void SetContraint(Vector3 point, Vector2 uv)
        {
            m_du = uv.x - point.x;
            m_dv = uv.y - point.y;
        }
    }

}
