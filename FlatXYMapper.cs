using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

public class FlatXYMapper : IUVMapper {

    public Vector2 GetUV(Vector3 point)
    {
        float u = point.x + 0.5f;
        float v = point.y;
        return new Vector2 (u, v);
    }

}

}
