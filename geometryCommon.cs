using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeometryCommonNS
{
    public class GeometryCommon
    {
        //1 above, -1 below, 0 - on line
        public int checkSidePointLine2D(Vector2 line, Vector2 point) {
            float y = (line.x * point.x) + line.y;
            if (point.y > y)
            {
                return 1;
            } else
            {
                if (point.y < y)
                {
                    return -1;
                }
            }

            return 0;
        }

        public float angleBeetweenLines(Vector2 line1, Vector2 line2) {
            float m1 = line1.x;
            float m2 = line2.x;

            return Mathf.Atan(Mathf.Abs((m2 - m1) / (1 + (m1 * m2))));
        }

        public float rotatePointByAngle(Vector2 point)
        {
            return 0f;
        }
    }
}
