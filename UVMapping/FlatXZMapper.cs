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

    //
    // Angle in radians
    //
    public void SetYRotation (float angle) {

        float cos = Mathf.Cos (Mathf.Deg2Rad * angle);
        float sin = Mathf.Sin (Mathf.Deg2Rad * angle);

        m_uX = sin;
        m_uZ = cos;

        m_vX =-cos;
        m_vZ = -sin;
    }

    public Vector2 GetUV(Vector3 point)
    {
        float u = m_uX * point.x + m_uZ * point.z + 0.5f;
        float v = m_vX * point.x + m_vZ * point.z + 0.5f;
        return new Vector2 (u, v);
    }

}

}
