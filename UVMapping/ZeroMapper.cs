using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    //
    //  Always return (u,v) = (0,0)
    //
    public class ZeroMapper : IUVMapper {

        public Vector2 GetUV(Vector3 point)
        {
            return new Vector2 (0f, 0f);
        }

        public void SetRotation(float angleRad)
        {
        }

        public void SetConstraint(Vector3 point, Vector2 uv)
        {
        }
    }
}
