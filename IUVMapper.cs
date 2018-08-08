using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public interface IUVMapper {

        Vector2 GetUV(Vector3 point);

        void SetRotation (float angleRad);

        void SetConstraint (Vector3 point, Vector2 uv);

    }

}
