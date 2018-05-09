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

        //
        // UV rotation
        //

        public static void SetRotation(ref List<PointList> list, float angleRad)
        {
            for(int i=0; i<list.Count; i++) {
                list [i].UVMapper.SetRotation(angleRad);
            }
        }

        //
        // UV Continuity
        //
        public static void Connect(ref List<PointList> list)
        {
            for(int i=1; i<list.Count; i++) {
                Connect(list [i], list[i-1]);
            }
        }

        //
        // Update pointsA UV Mapping to get continuity with pointsB UVMapping
        //
        public static void Connect(PointList pointsA, PointList pointsB)
        {
            // Check uids identity
            for (int i = 0; i < pointsA.Count; i++) {
                for (int j = 0; j < pointsB.Count; j++) {
                    if (pointsA.Uid (i) == pointsB.Uid (j)) {
                        Vector2 uv = pointsB.UVMapper.GetUV (pointsB [j]);
                        pointsA.UVMapper.SetConstraint(pointsA[i], uv);
                        return;
                    }
                }
            }

            float epsilon = 0.001f;

            // Check coordinates identity
            for (int i = 0; i < pointsA.Count; i++) {
                for (int j = 0; j < pointsB.Count; j++) {
                    float d = Vector3.Distance (pointsA [i], pointsB[j]);
                    if (d < epsilon) {
                        Vector2 uv = pointsB.UVMapper.GetUV (pointsB [j]);
                        pointsA.UVMapper.SetConstraint(pointsA[i], uv);
                        return;
                    }
                }
            }
        }


    }

}
