using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public interface IUVMapper {

        Vector2 GetUV(Vector3 point);

    }

}
