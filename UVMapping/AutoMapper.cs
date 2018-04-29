using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plasticine {

    public class AutoMapper {

        //
        // Get the best UV Mapper
        //
        private static IUVMapper ComputeUVMapper(PointList points)
        {
            IUVMapper result = null;
            Vector3 normal = points.ComputeNormal ().normalized;
            if (Mathf.Abs (normal.y) > 0.95f) {
                result = new FlatXZMapper ();
            } else if (Mathf.Abs (normal.x) > 0.95f) {
                result = new FlatYZMapper ();
            } else if (Mathf.Abs (normal.z) > 0.95f) {
                result = new FlatXYMapper ();
            } else {
                // More computation needed
                FlatMapper mapper = new FlatMapper ();
                mapper.InitLastSideU (points);
                mapper.SetConstraint (new Vector3 (-0.5f, 0, 0), new Vector2 (0, 0));
                result = mapper;
            }
            return result;
        }

        //
        // Find the best Axis Aligned UV Mapper
        //
        public static void ComputeUVMapper(ref PointList points)
        {
            points.UVMapper = ComputeUVMapper (points);
        }

        //
        //
        //
        public static void ComputeUVMapper(ref List<PointList> list)
        {
            for(int i=0; i<list.Count; i++) {
                list [i].UVMapper = ComputeUVMapper(list[i]);
            }
        }

    }

}
